#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aColor;
out vec3 ourColor;
void main()
{
	vec3 flip = vec3(aPos.x * -1, aPos.y * -1, aPos.z * -1);
	gl_Position = vec4(flip, 1.0);
	ourColor = aColor;
}