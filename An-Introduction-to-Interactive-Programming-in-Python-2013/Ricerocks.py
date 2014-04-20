# program template for Spaceship
import simplegui
import math
import random

# globals for user interface
WIDTH = 800
HEIGHT = 600
score = 0
lives = 3
time = 0.5
started = False


class ImageInfo:
    def __init__(self, center, size, radius = 0, lifespan = None, animated = False):
        self.center = center
        self.size = size
        self.radius = radius
        if lifespan:
            self.lifespan = lifespan
        else:
            self.lifespan = float('inf')
        self.animated = animated

    def get_center(self):
        return self.center

    def get_size(self):
        return self.size

    def get_radius(self):
        return self.radius

    def get_lifespan(self):
        return self.lifespan

    def get_animated(self):
        return self.animated

    
# art assets created by Kim Lathrop, may be freely re-used in non-commercial projects, please credit Kim
    
# debris images - debris1_brown.png, debris2_brown.png, debris3_brown.png, debris4_brown.png
#                 debris1_blue.png, debris2_blue.png, debris3_blue.png, debris4_blue.png, debris_blend.png
debris_info = ImageInfo([320, 240], [640, 480])
debris_image = simplegui.load_image("http://commondatastorage.googleapis.com/codeskulptor-assets/lathrop/debris2_blue.png")

# nebula images - nebula_brown.png, nebula_blue.png
nebula_info = ImageInfo([400, 300], [800, 600])
nebula_image = simplegui.load_image("http://commondatastorage.googleapis.com/codeskulptor-assets/lathrop/nebula_blue.f2013.png")

# splash image
splash_info = ImageInfo([200, 150], [400, 300])
splash_image = simplegui.load_image("http://commondatastorage.googleapis.com/codeskulptor-assets/lathrop/splash.png")

# ship image
ship_info = ImageInfo([45, 45], [90, 90], 35)
ship_image = simplegui.load_image("http://commondatastorage.googleapis.com/codeskulptor-assets/lathrop/double_ship.png")

# missile image - shot1.png, shot2.png, shot3.png
missile_info = ImageInfo([5,5], [10, 10], 3, 50)
missile_image = simplegui.load_image("http://commondatastorage.googleapis.com/codeskulptor-assets/lathrop/shot2.png")

# asteroid images - asteroid_blue.png, asteroid_brown.png, asteroid_blend.png
asteroid_info = ImageInfo([45, 45], [90, 90], 40)
asteroid_image = simplegui.load_image("http://commondatastorage.googleapis.com/codeskulptor-assets/lathrop/asteroid_blue.png")

# animated explosion - explosion_orange.png, explosion_blue.png, explosion_blue2.png, explosion_alpha.png
explosion_info = ImageInfo([64, 64], [128, 128], 17, 24, True)
explosion_image = simplegui.load_image("http://commondatastorage.googleapis.com/codeskulptor-assets/lathrop/explosion_alpha.png")

# sound assets purchased from sounddogs.com, please do not redistribute
soundtrack = simplegui.load_sound("http://commondatastorage.googleapis.com/codeskulptor-assets/sounddogs/soundtrack.mp3")
missile_sound = simplegui.load_sound("http://commondatastorage.googleapis.com/codeskulptor-assets/sounddogs/missile.mp3")
missile_sound.set_volume(.5)
ship_thrust_sound = simplegui.load_sound("http://commondatastorage.googleapis.com/codeskulptor-assets/sounddogs/thrust.mp3")
explosion_sound = simplegui.load_sound("http://commondatastorage.googleapis.com/codeskulptor-assets/sounddogs/explosion.mp3")

# helper functions to handle transformations
def angle_to_vector(ang):
    return [math.cos(ang), math.sin(ang)]

def dist(p, q):
    return math.sqrt((p[0] - q[0]) ** 2 + (p[1] - q[1]) ** 2)

# draw and update rocks and missiles
def process_sprite_group(sprite_group, canvas):
    for sprite in set(sprite_group):
        sprite.draw(canvas)
        sprite.update()
        
        if sprite.update():
            sprite_group.discard(sprite)

# check for collisions between rocks and the ship
def group_collide(sprite_group, other_object):
    for sprite in set(sprite_group):
        if sprite.collide(other_object):
            sprite_group.discard(sprite)
            explosion_group.add(Sprite(sprite.get_position(), sprite.vel, 0, 0,
                                       explosion_image, explosion_info, explosion_sound))
            return True
    return False  

# check for collisions between missiles and rocks
def group_group_collide(sprite_group0, sprite_group1):
    global score
    for sprite in set(sprite_group0):
        if group_collide(sprite_group1, sprite):
            sprite_group1.discard(sprite)
            sprite_group0.discard(sprite)
            score += 1
            
# Ship class
class Ship:
    def __init__(self, pos, vel, angle, image, info):
        self.pos = [pos[0], pos[1]]
        self.vel = [vel[0], vel[1]]
        self.thrust = False
        self.angle = angle
        self.angle_vel = 0
        self.image = image
        self.image_center = info.get_center()
        self.image_size = info.get_size()
        self.radius = info.get_radius()
        
    def draw(self, canvas):
        if self.thrust:
            canvas.draw_image(self.image, (self.image_center[0] + 90, self.image_center[1]), 
                              self.image_size, self.pos, 
                              self.image_size, self.angle)
        else:
            canvas.draw_image(self.image, self.image_center, self.image_size, 
                              self.pos, self.image_size, self.angle) 

    def get_position(self):
        return self.pos
    
    def get_radius(self):
        return self.radius
    
    def update(self):
        self.pos[0] = (self.pos[0] + self.vel[0]) % WIDTH
        self.pos[1] = (self.pos[1] + self.vel[1]) % HEIGHT
        self.angle += self.angle_vel
        forward = angle_to_vector(self.angle)
        
        self.vel[0] *= (1 - 0.004)
        self.vel[1] *= (1 - 0.004)
        
        if self.thrust:
            self.vel[0] += forward[0] * 0.05
            self.vel[1] += forward[1] * 0.05

    def increment_angle_vel(self):
        self.angle_vel += .05
    
    def decrement_angle_vel(self):
        self.angle_vel -= .05            

    def thrusters(self, activated):
        if activated:
            self.thrust = True
            ship_thrust_sound.set_volume(0.5)
            ship_thrust_sound.play()
        else:
            self.thrust = False
            ship_thrust_sound.rewind()
            
    def shoot(self):
        global a_missile
        x_pos = self.pos[0] + 45 * angle_to_vector(self.angle)[0]
        y_pos = self.pos[1] + 45 * angle_to_vector(self.angle)[1]
        x_vel = self.vel[0] + angle_to_vector(self.angle)[0] * 7
        y_vel = self.vel[1] + angle_to_vector(self.angle)[1] * 7
        
        missile_group.add(Sprite([x_pos, y_pos], [x_vel, y_vel], 0, 0,
                                 missile_image, missile_info, missile_sound))

    
# Sprite class
class Sprite:
    def __init__(self, pos, vel, ang, ang_vel, image, info, sound = None):
        self.pos = [pos[0],pos[1]]
        self.vel = [vel[0],vel[1]]
        self.angle = ang
        self.angle_vel = ang_vel
        self.image = image
        self.image_center = info.get_center()
        self.image_size = info.get_size()
        self.radius = info.get_radius()
        self.lifespan = info.get_lifespan()
        self.animated = info.get_animated()
        self.age = 0
        
        if sound:
            sound.rewind()
            sound.play()
   
    def draw(self, canvas):
        if self.animated:  
            canvas.draw_image(self.image,
                              [self.image_center[0] + self.age * self.image_size[0],
                               self.image_center[1]],
                              self.image_size, self.pos, self.image_size, self.angle) 
        else:
            canvas.draw_image(self.image, self.image_center, self.image_size, 
                              self.pos, self.image_size, self.angle) 

    def get_position(self):
        return self.pos
    
    def get_radius(self):
        return self.radius
    
    def update(self):
        self.pos[0] = (self.pos[0] + self.vel[0]) % WIDTH
        self.pos[1] = (self.pos[1] + self.vel[1]) % HEIGHT
        self.angle += self.angle_vel       
        self.age += 1
        
        return self.age >= self.lifespan
        
    def collide(self, other_object):
        return dist(self.pos, other_object.get_position()) <= self.radius + other_object.get_radius() 
               
    
# keys: keydown True/False, clockwise True/False
def keydown(key):
    if started:
        if key == simplegui.KEY_MAP['left']:
            my_ship.decrement_angle_vel()
        
        if key == simplegui.KEY_MAP['right']:
            my_ship.increment_angle_vel()
        
        if key == simplegui.KEY_MAP["up"]:
            my_ship.thrusters(True)
        
        if key == simplegui.KEY_MAP["space"]:
            my_ship.shoot()
    
def keyup(key):
    if started:
        if key == simplegui.KEY_MAP['left']:
            my_ship.increment_angle_vel()
        
        if key == simplegui.KEY_MAP['right']:
            my_ship.decrement_angle_vel()
        
        if key == simplegui.KEY_MAP["up"]:
            my_ship.thrusters(False)

# mouseclick handlers that reset UI and conditions whether splash image is drawn
def click(pos):
    global started, my_ship, score, lives
    center = [WIDTH / 2, HEIGHT / 2]
    size = splash_info.get_size()
    inwidth = (center[0] - size[0] / 2) < pos[0] < (center[0] + size[0] / 2)
    inheight = (center[1] - size[1] / 2) < pos[1] < (center[1] + size[1] / 2)
    
    if (not started) and inwidth and inheight:
        started = True
        my_ship = Ship([WIDTH / 2, HEIGHT / 2], [0, 0], 0, ship_image, ship_info)
        score = 0
        lives = 3
        soundtrack.set_volume(0.3)
        soundtrack.play()
        
def draw(canvas):
    global score, lives, time, started, rock_group, my_ship
    
    # animiate background
    time += 1
    wtime = (time / 4) % WIDTH
    center = debris_info.get_center()
    size = debris_info.get_size()
    canvas.draw_image(nebula_image, nebula_info.get_center(), nebula_info.get_size(), [WIDTH / 2, HEIGHT / 2], [WIDTH, HEIGHT])
    canvas.draw_image(debris_image, center, size, (wtime - WIDTH / 2, HEIGHT / 2), (WIDTH, HEIGHT))
    canvas.draw_image(debris_image, center, size, (wtime + WIDTH / 2, HEIGHT / 2), (WIDTH, HEIGHT))

    # draw and update ship
    my_ship.draw(canvas)
    my_ship.update()   
    
    # call process_sprite_group function to draw and update
    # rocks, missiles and explosions
    process_sprite_group(rock_group, canvas)
    process_sprite_group(missile_group, canvas)
    process_sprite_group(explosion_group, canvas)
    
    # collisions between missiles and rocks
    group_group_collide(missile_group, rock_group)
    
    # draw score and lives
    canvas.draw_text("Score:", (690, 40), 25, "White")
    canvas.draw_text(str(score), (760, 40), 25, "White")
    canvas.draw_text("Lives:", (25, 40), 25, "White")
    canvas.draw_text(str(lives), (95, 40), 25, "White")
    
    # draw splash screen if not started
    if not started:
        canvas.draw_image(splash_image, splash_info.get_center(), 
                          splash_info.get_size(), [WIDTH / 2, HEIGHT / 2], 
                          splash_info.get_size())
   
    # check if game over    
    if group_collide(rock_group, my_ship):
        lives -= 1
        
    if lives == 0:
        started = False
        rock_group = set([])
        my_ship.thrust = False
        ship_thrust_sound.rewind()
        soundtrack.rewind()
        
# timer handler that spawns a rock    
def rock_spawner():
    global rock_group
    if len(rock_group) < 12 and started:
        position = (random.randint(0, WIDTH), random.randint(0, HEIGHT))
        velocity = ((random.random() - random.random()) * (score * 0.05),
                    (random.random() - random.random()) * (score * 0.05))
        angle = random.random() * math.pi*2 
        angle_vel = (random.random() - random.random()) * 0.1
        
        if dist(position, my_ship.get_position()) > 120:
            rock_group.add(Sprite(position, velocity, angle, angle_vel,
                                  asteroid_image, asteroid_info))        
        
    
# initialize frame
frame = simplegui.create_frame("Asteroids", WIDTH, HEIGHT)

# initialize ship and three groups
my_ship = Ship([WIDTH / 2, HEIGHT / 2], [0, 0], 0, ship_image, ship_info)
rock_group = set([])
missile_group = set([])
explosion_group = set([])

# register handlers
frame.set_keydown_handler(keydown)
frame.set_keyup_handler(keyup)
frame.set_mouseclick_handler(click)
frame.set_draw_handler(draw)


timer = simplegui.create_timer(1000.0, rock_spawner)

# get things rolling
timer.start()
frame.start()

