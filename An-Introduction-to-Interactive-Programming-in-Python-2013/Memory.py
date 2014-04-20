# implementation of card game - Memory

import simplegui
import random

# helper function to initialize globals
def new_game():
    global NO_OF_CARDS, EXPOSED, card_no, state, turns 
    card_no = range(8)*2
    NO_OF_CARDS = range(len(card_no))
    EXPOSED = [False] * len(NO_OF_CARDS)
    
    """Everyday i'm shuffaling (these card numbers)"""
    random.shuffle(card_no)
    state = 0
    turns = 0
    label.set_text("Turns = " + str(turns))    

# define event handlers
def mouseclick(pos):
    # add game state logic here
    """ """
    global NO_OF_CARDS, EXPOSED, card_no, state, turns, index0, index1
    i = pos[0] // 50
    if not EXPOSED[i]:
        if state == 0:
            state = 1
            EXPOSED[i] = True
            index0 = i
        elif state == 1:
            state = 2
            EXPOSED[i] = True
            index1 = i
            turns += 1
        else:
            state = 1
            if card_no[index0] != card_no[index1]:
                EXPOSED[index0] = False
                EXPOSED[index1] = False
            EXPOSED[i] = True
            index0 = i
        label.set_text("Turns = " + str(turns))
                
                     
# cards are logically 50x100 pixels in size    
def draw(canvas):
    """Draw numbers"""
    for number in range(len(card_no)):
        xpos_no = (number * 50) + 13
        canvas.draw_text(str(card_no[number]), (xpos_no, 65), 45,
                         "rgba(0,255,0,1.0)")
    
    """Draw green cards on top of the numbers. If exposed make them 
    transparent using RGBA and setting alpha to 0.0"""
    for c in range(len(NO_OF_CARDS)):  
            if EXPOSED[c]:
                color = "rgba(0,0,0,0.0)"
            else:
                color = "rgba(0,200,0,1.0)"           
            xpos_c = [(c * 50), (c * 50 + 50), (c * 50 + 50), (c * 50)]
            canvas.draw_polygon([[xpos_c[0], 0], [xpos_c[1], 0],
                                 [xpos_c[2], 100], [xpos_c[3], 100]],
                                 1, "White", color)
           
        
# create frame and add a button and labels
frame = simplegui.create_frame("Memory", 800, 100)
frame.add_button("Restart", new_game)
label = frame.add_label("Turns = 0")


# register event handlers
frame.set_mouseclick_handler(mouseclick)
frame.set_draw_handler(draw)


# get things rolling
new_game()
frame.start()

# Always remember to review the grading rubric
