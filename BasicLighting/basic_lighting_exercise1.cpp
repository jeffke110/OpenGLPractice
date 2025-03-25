glm::vec3 lightPos(0.0f, 0.0f, 2.0f);
lightPos = glm::vec3(
    cos(glfwGetTime()) * 5, // x position
    lightPos.y,             // y position (constant or you can modify it for vertical movement)
    sin(glfwGetTime()) * 5  // z position
);

