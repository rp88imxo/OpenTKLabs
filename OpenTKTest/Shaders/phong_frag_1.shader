#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;

// --- Материал ---
struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;
};
uniform Material material;

// --- Направленный свет ---
struct DirLight {
    vec3 direction; // Направление ОТ источника света
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
uniform DirLight dirLight;

// --- Точечные источники ---
#define NR_POINT_LIGHTS 3 // Количество точечных источников
struct PointLight {
    vec3 position;

    float constant;
    float linear;
    float quadratic;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
uniform PointLight pointLights[NR_POINT_LIGHTS];


struct SpotLight {
    vec3 position;
    vec3 direction; // Направление, куда светит
    float cutOff;       // Косинус внутреннего угла
    float outerCutOff;  // Косинус внешнего угла

    float constant;
    float linear;
    float quadratic;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
uniform SpotLight spotLight; // <<< НОВОЕ (пока только один)

uniform vec3 viewPos; // Позиция камеры

// Функция расчета вклада направленного света (без изменений)
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction); // Направление К источнику

    // Diffuse
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * (diff * material.diffuse);

    // Specular
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * (spec * material.specular);

    // Ambient
    vec3 ambient = light.ambient * material.ambient;

    return (ambient + diffuse + specular);
}

// Функция расчета вклада точечного света (без изменений)
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos); // Направление К источнику

    // Diffuse
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * (diff * material.diffuse);

    // Specular
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * (spec * material.specular);

    // Ambient
    vec3 ambient = light.ambient * material.ambient;

    // Attenuation (Затухание)
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    return (ambient + diffuse + specular);
}

// --- Функция расчета вклада прожектора --- <<< НОВАЯ ФУНКЦИЯ
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos); // Направление К источнику

    // Проверка, находится ли фрагмент внутри конуса
    float theta = dot(lightDir, normalize(-light.direction)); // Угол между вектором к фрагменту и направлением света
    float epsilon = light.cutOff - light.outerCutOff;      // Разница между косинусами углов
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0); // Плавное затухание по краям конуса

    // Если фрагмент вне внешнего конуса, вклад света равен 0 (кроме ambient, его не ограничиваем конусом)
    if(theta < light.outerCutOff)
    {
         // Рассчитываем только затухание для ambient
         float distance = length(light.position - fragPos);
         float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
         vec3 ambient = light.ambient * material.ambient * attenuation;
         return ambient; // Возвращаем только ambient
    }

    // Расчеты как у точечного источника
    // Diffuse
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = light.diffuse * (diff * material.diffuse);

    // Specular
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * (spec * material.specular);

    // Ambient
    vec3 ambient = light.ambient * material.ambient;

    // Attenuation
    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    // Применяем затухание ко всем компонентам
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;

    // Применяем интенсивность конуса к diffuse и specular
    diffuse *= intensity;
    specular *= intensity;

    return (ambient + diffuse + specular);
}


void main()
{
    vec3 norm = normalize(Normal); // Нормализуем интерполированную нормаль
    vec3 viewDir = normalize(viewPos - FragPos); // Вектор от фрагмента к камере

    // --- Расчет направленного света ---
    vec3 result = CalcDirLight(dirLight, norm, viewDir);

    // --- Расчет точечных источников ---
    for(int i = 0; i < NR_POINT_LIGHTS; i++)
        result += CalcPointLight(pointLights[i], norm, FragPos, viewDir);

    // --- Расчет прожектора --- <<< НОВЫЙ ВЫЗОВ
    result += CalcSpotLight(spotLight, norm, FragPos, viewDir);

    FragColor = vec4(result, 1.0);
}