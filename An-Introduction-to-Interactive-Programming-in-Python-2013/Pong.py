# Implementation of classic arcade game Pong

import simplegui
import random

# initialize globals - pos and vel encode vertical info for paddles
WIDTH = 600
HEIGHT = 400       
BALL_RADIUS = 20
PAD_WIDTH = 8
PAD_HEIGHT = 80
HALF_PAD_WIDTH = PAD_WIDTH / 2
HALF_PAD_HEIGHT = PAD_HEIGHT / 2
LEFT = False
RIGHT = True
ball_pos = [WIDTH / 2, HEIGHT / 2]
ball_vel = [0, 0]
paddle1_vel = 0
paddle2_vel = 0
paddle1_pos = float(HEIGHT / 2)
paddle2_pos = float(HEIGHT / 2)

# initialize ball_pos and ball_vel for new bal in middle of table
# if direction is RIGHT, the ball's velocity is upper right, else upper left
def spawn_ball(direction):
    global ball_pos, ball_vel # these are vectors stored as lists
    ball_pos = [WIDTH / 2, HEIGHT / 2]
    if direction:
        ball_vel = [random.randrange(120, 240) // 60, -random.randrange(60, 180) // 60]
    else:
        ball_vel = [-random.randrange(120, 240) // 60, -random.randrange(60, 180) // 60]
     
# define event handlers
def new_game():
    global paddle1_pos, paddle2_pos, paddle1_vel, paddle2_vel  # these are numbers
    global score1, score2  # these are ints
    paddle1_pos = float(HEIGHT / 2)
    paddle2_pos = float(HEIGHT / 2)
    paddle1_vel = 0.0
    paddle2_vel = 0.0
    score1 = 0
    score2 = 0
    direction = random.randint(0, 1)
    
    if direction == 0:
        side = RIGHT
    else:
        side = LEFT
    spawn_ball(side)
    
def draw(c):
    global score1, score2, paddle1_pos, paddle2_pos, ball_pos, ball_vel
    global paddle1_vel, paddle2_vel
        
    # draw mid line and gutters
    c.draw_line([WIDTH / 2, 0],[WIDTH / 2, HEIGHT], 1, "White")
    c.draw_line([PAD_WIDTH, 0],[PAD_WIDTH, HEIGHT], 1, "White")
    c.draw_line([WIDTH - PAD_WIDTH, 0],[WIDTH - PAD_WIDTH, HEIGHT], 1, "White")
        
    # update ball
    ball_pos[0] += ball_vel[0]
    ball_pos[1] += ball_vel[1]
    
    # collide and reflect off of top and bottom side of canvas
    if ball_pos[1] <= BALL_RADIUS:
        ball_vel[1] = - ball_vel[1]
    if ball_pos[1] >= (HEIGHT - 1) - BALL_RADIUS:
        ball_vel[1] = - ball_vel[1]
    
    # collide and reflect from the paddles
    # else collide with left or right gutters and respawn ball
    # and give appropriate score to the correct player
    if ball_pos[0] < PAD_WIDTH + BALL_RADIUS:
        if ball_pos[1] > paddle1_pos - HALF_PAD_HEIGHT and ball_pos[1] < paddle1_pos + HALF_PAD_HEIGHT:
            ball_vel[0] *= - 1.1 
            ball_vel[1] *= 1.1
        else:
            score2 += 1
            spawn_ball(RIGHT)
    if ball_pos[0] > ((WIDTH -1) - PAD_WIDTH) - BALL_RADIUS:
        if ball_pos[1] > paddle2_pos - HALF_PAD_HEIGHT and ball_pos[1] < paddle2_pos + HALF_PAD_HEIGHT:
            ball_vel[0] *= - 1.1 
            ball_vel[1] *= 1.1
        else:
            score1 += 1
            spawn_ball(LEFT)
    #print ball_vel[0], ball_vel[1]
    
    # draw ball
    c.draw_circle(ball_pos, BALL_RADIUS, 2, "White", "White")
    
    # update paddles' vertical position, keep paddles on the screen
    paddle1_pos += paddle1_vel
    if paddle1_pos - HALF_PAD_HEIGHT <= 0:
        paddle1_pos = HALF_PAD_HEIGHT
    elif paddle1_pos + HALF_PAD_HEIGHT >= HEIGHT - 1:
        paddle1_pos = (HEIGHT - 1) - HALF_PAD_HEIGHT
        
    paddle2_pos += paddle2_vel
    if paddle2_pos - HALF_PAD_HEIGHT <= 0:
        paddle2_pos = HALF_PAD_HEIGHT
    elif paddle2_pos + HALF_PAD_HEIGHT >= HEIGHT - 1:
        paddle2_pos = (HEIGHT - 1) - HALF_PAD_HEIGHT
    
    # draw paddles
    """
    Some people use c.draw_line but I personaly don't think that's
    the correct way of doing this. Therefore I'm using c.draw_polygon and
    hopefuly if whoever is evaluating doesn't understand will do 
    after reading my explanation.
    """
    c.draw_polygon([[0, paddle1_pos - HALF_PAD_HEIGHT], 
                   [PAD_WIDTH, paddle1_pos - HALF_PAD_HEIGHT],
                   [PAD_WIDTH, paddle1_pos + HALF_PAD_HEIGHT],
                   [0, paddle1_pos + HALF_PAD_HEIGHT]],
                   1, "White", "White")
    """
    This draws the paddle on the left. Each square bracket starts a point and
    as you know the far left starts at 0 so to draw the top left point of 
    the rectangle one must start at 0, then point the position of the paddle
    and subtract its half height - these are the coordinates.
    Now all you need to do is give the coordinates of the other 3 points
    in order - top left > top right > bottom right > bottom left - and that's it
    Now all that needs to be done is to draw the right paddle.
    """
    c.draw_polygon([[WIDTH - PAD_WIDTH, paddle2_pos - HALF_PAD_HEIGHT],
                    [WIDTH, paddle2_pos - HALF_PAD_HEIGHT],
                    [WIDTH, paddle2_pos + HALF_PAD_HEIGHT],
                    [WIDTH - PAD_WIDTH, paddle2_pos + HALF_PAD_HEIGHT]],
                    1, "White", "White")
    """
    Hopefuly you understand how the right paddle is drawn. You have to subtract
    the canvas width with the paddle width to get the top left point of the paddle.
    Then use the canvas width to help you create the top and bottom right points.
    The bottom left point is the same as the top but instead of subtracting
    you add the paddle half height - just like the left paddle.
    
    It took me about an hour or more trying to understand draw_polygon :S
    It's not the best explenation but I managed to do it following this logic.
    That number "1" is the width of the paddle, fyi.
    """
                    
    # draw scores
    if score1 < 10:
        c.draw_text(str(score1), [255, 50], 40, "White")
    elif score1 > 99:
        c.draw_text(str(score1), [205, 50], 40, "White")
    else:
        c.draw_text(str(score1), [230, 50], 40, "White")
    c.draw_text(str(score2), [320, 50], 40, "White")
    
        
def keydown(key):
    global paddle1_vel, paddle2_vel
    acc = 10
    if key == simplegui.KEY_MAP["s"]:
        paddle1_vel += acc 
    elif key == simplegui.KEY_MAP["w"]:
        paddle1_vel -= acc   
    if key == simplegui.KEY_MAP["down"]:
        paddle2_vel += acc
    elif key == simplegui.KEY_MAP["up"]:
        paddle2_vel -= acc    
    
   
def keyup(key):
    global paddle1_vel, paddle2_vel
    acc = 0
    if key == simplegui.KEY_MAP["s"]:
        paddle1_vel = acc
    elif key == simplegui.KEY_MAP["w"]:
        paddle1_vel = acc
    elif key == simplegui.KEY_MAP["down"]:
        paddle2_vel = acc
    elif key == simplegui.KEY_MAP["up"]:
        paddle2_vel = acc    


# create frame
frame = simplegui.create_frame("Pong", WIDTH, HEIGHT)
frame.set_draw_handler(draw)
frame.set_keydown_handler(keydown)
frame.set_keyup_handler(keyup)
button1 = frame.add_button('Restart', new_game, 100)

# start frame
new_game()
frame.start()

