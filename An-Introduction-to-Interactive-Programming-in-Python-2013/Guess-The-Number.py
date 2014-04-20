# template for "Guess the number" mini-project
# input will come from buttons and an input field
# all output for the game will be printed in the console
import simplegui
import random
import math

# initialize global variables used in your code
#num_range = 0
num_range = 100
number = 0
no_of_guesses = 0

# helper function to start and restart the game
def new_game():
    global number
    global no_of_guesses
    number = random.randint(0, num_range)
    if num_range == 100:
        no_of_guesses = 7
    elif num_range == 1000:
        no_of_guesses = 10
    elif num_range != 100 or num_range != 1000:
        print "Number range has been messed with!"    
    print "New game. Range is from 0 to", num_range
    print "Number of guesses is", no_of_guesses
        
    # remove this when you add your code    

# define event handlers for control panel
def range100():
    # button that changes range to range [0,100) and restarts
    global num_range
    num_range = 100
    new_game()
    print ""
    # remove this when you add your code    

def range1000():
    # button that changes range to range [0,1000) and restarts
    global num_range
    num_range = 1000
    new_game()
    print ""
    # remove this when you add your code    
    
def input_guess(guess):
    # main game logic goes here	
    global num_range
    #if num_range == 0:
    #   print "You must choose a DESTINY first! \n"
    #   return input_guess
    global no_of_guesses
    no_of_guesses -= 1
    guess = float(guess)
    print "Your guess was", guess
    print "Your number of remaining guesses is", no_of_guesses
    if guess > number:
        print "Lower! \n" 
    elif guess < number:
        print "Higher! \n"
    else:
        print "Correct! \n"
        new_game()
        print ""
    if guess > num_range or guess < 0:
        print "It's between 0 and", str(num_range) + ", silly! \n"
    if no_of_guesses <= 0:
        print "You're out of guesses, the number was", number, "\n"
        new_game()
        print ""
    # remove this when you add your code

    
# create frame
f = simplegui.create_frame("Guess the number", 200, 200)


# register event handlers for control elements
f.add_button("Range is [0, 100)", range100, 200)
f.add_button("Range is [0, 1000)", range1000, 200)
f.add_input("Enter a guess", input_guess, 200)

# call new_game and start frame
new_game()
print ""
#print "Choose your destiny!"
f.start()

# always remember to check your completed program against the grading rubric

