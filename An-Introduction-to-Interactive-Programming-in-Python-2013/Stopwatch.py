# template for "Stopwatch: The Game"
import simplegui

# define global variables
clock = "0:00.0"
active = 0
win_count = 0
no_of_games = 0
t = 0

# define helper function format that converts time
# in tenths of seconds into formatted string A:BC.D
def format(t):
    global clock
    min = t // 600
    tens_of_sec = ((t // 10) % 60) // 10
    sec_in_exc = ((t // 10) % 60) % 10
    millisec = str(t)[-1]
    clock = str(min) + ":" + str(tens_of_sec) + str(sec_in_exc) + "." + str(millisec)
     
# define event handlers for buttons; "Start", "Stop", "Reset"
def start():
    global active
    active = 1
    timer.start()
    
def stop():
    global active, win_count, no_of_games
    if active == 1:
        no_of_games += 1
    if clock[-1] == str(0) and active == 1:
        win_count += 1
    active = 0
    timer.stop()
    
def reset():
    global active, t, win_count, no_of_games
    active = 0
    t = 0
    win_count = 0
    no_of_games = 0
    timer.stop()
    format(t)    
    
# define event handler for timer with 0.1 sec interval
def timer():
    global t
    t += 1
    format(t)
    
# define draw handler
def draw(canvas):
    canvas.draw_text(clock, [100, 180], 70, "White")
    canvas.draw_text(str(win_count) + "/" + str(no_of_games), [280, 50], 40, "Green")

# create frame
frame = simplegui.create_frame("Stopwatch", 400, 300)
timer = simplegui.create_timer(100, timer)

# register event handlers
frame.add_button("Start", start, 100)
frame.add_button("Stop", stop, 100)
frame.add_button("Reset", reset, 100)
frame.set_draw_handler(draw)

# start frame
frame.start()

# Please remember to review the grading rubric

