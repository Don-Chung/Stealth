cmake_minimum_required(VERSION 3.21)
project(Server)

set(CMAKE_CXX_STANDARD 14)

include_directories(/usr/include/mysql)
add_executable(Server main.cpp)
target_link_libraries(Server -lpthread)
target_link_libraries(Server -lmysqlclient)