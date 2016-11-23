"""
Homework 1. Task 1.

Author: Mikhail Kita, group 371
"""

from OpenGL.GL import *
from OpenGL.GLU import *
from OpenGL.GLUT import *
from math import tan, pi

perspective = False


def draw():
    glClear(GL_COLOR_BUFFER_BIT)

    edge = 0.5

    # perspective projection
    if perspective:
        angle = 90.0
        f = -(edge + 2 * edge / tan(pi * angle / 360))

        gluPerspective(angle, 1.0, 0.0, 1.0)
        glTranslatef(0.0, 0.0, f)

    glBegin(GL_QUADS)

    # left side
    glColor3f(1.0, 0.0, 0.0)
    glVertex3f(-edge, -edge, -edge)
    glVertex3f(-edge, edge, -edge)
    glVertex3f(-edge, edge, edge)
    glVertex3f(-edge, -edge, edge)

    # right side
    glColor3f(1.0, 0.5, 0.0)
    glVertex3f(edge, -edge, -edge)
    glVertex3f(edge, -edge, edge)
    glVertex3f(edge, edge, edge)
    glVertex3f(edge, edge, -edge)

    # bottom side
    glColor3f(1.0, 1.0, 0.0)
    glVertex3f(-edge, -edge, -edge)
    glVertex3f(-edge, -edge, edge)
    glVertex3f(edge, -edge, edge)
    glVertex3f(edge, -edge, -edge)

    # top side
    glColor3f(0.0, 1.0, 0.0)
    glVertex3f(-edge, edge, -edge)
    glVertex3f(-edge, edge, edge)
    glVertex3f(edge, edge, edge)
    glVertex3f(edge, edge, -edge)

    # back side
    glColor3f(0.0, 0.0, 1.0)
    glVertex3f(-edge, -edge, -edge)
    glVertex3f(edge, -edge, -edge)
    glVertex3f(edge, edge, -edge)
    glVertex3f(-edge, edge, -edge)

    glEnd()
    glutSwapBuffers()


glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGB)
glutInitWindowSize(400, 400)
glutInit(sys.argv)
glutCreateWindow(b"Cube")
glutDisplayFunc(draw)

glClearColor(0.0, 0.0, 0.0, 1.0)
glDisable(GL_LIGHTING)

glutMainLoop()
