#version 330 core
out vec4 FragColor;

in vec3 Normal;  // Interpolated normal vector from the vertex shader
in vec3 FragPos; // Fragment position in world space
in vec2 TexCoords; // Texture coordinates

// Material properties
struct Material {
    sampler2D diffuse;  // Diffuse texture map
    sampler2D specular; // Specular texture map
    float shininess;    // Shininess exponent for specular highlights
};

// Light properties
struct Light {
    vec3 position;   // Light source position in world space
    vec3 direction;  // Spotlight direction
    vec3 ambient;    // Ambient light color
    vec3 diffuse;    // Diffuse light color
    vec3 specular;   // Specular light color

    // Attenuation factors
    float constant;
    float linear;
    float quadratic;
    
    // Spotlight cutoff angles
    float cutOff;       // Inner cutoff angle (cosine value)
    float outerCutOff;  // Outer cutoff angle (cosine value for smooth transition)
};

// Uniform variables
uniform Light light;
uniform Material material;
uniform vec3 viewPos; // Camera/view position

void main()
{
    // --- Ambient lighting ---
    vec3 ambient = light.ambient * texture(material.diffuse, TexCoords).rgb;
    
    // --- Diffuse lighting ---
    vec3 norm = normalize(Normal); // Normalize the normal vector
    vec3 lightDir = normalize(light.position - FragPos); // Direction from fragment to light source
    float diff = max(dot(norm, lightDir), 0.0); // Compute the diffuse intensity
    vec3 diffuse = light.diffuse * diff * texture(material.diffuse, TexCoords).rgb;
    
    // --- Specular lighting ---
    vec3 viewDir = normalize(viewPos - FragPos); // Direction from fragment to viewer
    vec3 reflectDir = reflect(-lightDir, norm); // Reflect light direction around the normal
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess); // Specular intensity
    vec3 specular = light.specular * spec * texture(material.specular, TexCoords).rgb;
    
    // --- Spotlight effect with soft edges ---
    float theta = dot(lightDir, normalize(-light.direction)); // Angle between light direction and spotlight direction
    float epsilon = (light.cutOff - light.outerCutOff); // Range between inner and outer cutoff
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0); // Interpolation for soft edges
    
    diffuse  *= intensity;
    specular *= intensity;
    
    // --- Attenuation (light falloff over distance) ---
    float distance = length(light.position - FragPos); // Compute distance between fragment and light source
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
    
    // Apply attenuation to all lighting components
    ambient  *= attenuation;
    diffuse  *= attenuation;
    specular *= attenuation;
    
    // Final color calculation
    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);
}
