//vertex shader

#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aColor;

out vec3 ourPosition;
uniform float xOffset;


void main()
{
	gl_Position = vec4(aPos.x + xOffset, aPos.y, aPos.z, 1.0);
	ourPosition = vec3(gl_Position.x, gl_Position.y, gl_Position.z);
}
// fragment shader

#version 330 core
out vec4 FragColor;
in vec3 ourPosition;
void main()
{
	FragColor = vec4(ourPosition, 1.0f);
}