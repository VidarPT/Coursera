# Mini-project #6 - Blackjack

import simplegui
import random

# load card sprite - 949x392 - source: jfitz.com
CARD_SIZE = (73, 98)
CARD_CENTER = (36.5, 49)
card_images = simplegui.load_image("http://commondatastorage.googleapis.com/codeskulptor-assets/cards.jfitz.png")

CARD_BACK_SIZE = (71, 96)
CARD_BACK_CENTER = (35.5, 48)
card_back = simplegui.load_image("http://commondatastorage.googleapis.com/codeskulptor-assets/card_back.png")    

# initialize some useful global variables
in_play = False
outcome = ""
score = 0
no_of_games = 0

# define globals for cards
SUITS = ('C', 'S', 'H', 'D')
RANKS = ('A', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K')
VALUES = {'A':1, '2':2, '3':3, '4':4, '5':5, '6':6, '7':7, '8':8, '9':9, 'T':10, 'J':10, 'Q':10, 'K':10}


# define card class
class Card:
    def __init__(self, suit, rank):
        if (suit in SUITS) and (rank in RANKS):
            self.suit = suit
            self.rank = rank
        else:
            self.suit = None
            self.rank = None
            print "Invalid card: ", suit, rank

    def __str__(self):
        return self.suit + self.rank

    def get_suit(self):
        return self.suit

    def get_rank(self):
        return self.rank

    def draw(self, canvas, pos):
        card_loc = (CARD_CENTER[0] + CARD_SIZE[0] * RANKS.index(self.rank), 
                    CARD_CENTER[1] + CARD_SIZE[1] * SUITS.index(self.suit))
        canvas.draw_image(card_images, card_loc, CARD_SIZE, 
                          [pos[0] + CARD_CENTER[0], pos[1] + CARD_CENTER[1]], CARD_SIZE)
        
# define hand class
class Hand:
    def __init__(self):
        # create a Hand object
        self.hand = []
            
    def __str__(self):
        # return a string representation of a hand
        str1 = ""
        for i in range (len(self.hand)):
            str1 = str1 + str(self.hand[i]) + " "                 
        return "Hand Contains " + str1
        
    def add_card(self, card):
        # add a card object to a hand
        self.hand.append(card)
        
    def get_value(self):
        # count aces as 1, if the hand has an ace,
        # then add 10 to hand value if it doesn't bust
        # compute the value of the hand, see Blackjack video
        hand_value = 0
        aces = 0
        for card in self.hand:
            hand_value += VALUES[card.get_rank()]        
            if card.get_rank() == "A":
                aces = 1
        if (hand_value + 10) <= 21 and aces == 1:
            hand_value = sum([hand_value, 10])
        return hand_value

    def draw(self, canvas, pos):
        for c in self.hand:
            if in_play:
                canvas.draw_image(card_back, CARD_BACK_CENTER, CARD_BACK_SIZE,
                                  [10 + CARD_BACK_CENTER[0], 330 + CARD_BACK_CENTER[1]],
                                  CARD_BACK_SIZE)  
            
            if in_play or not in_play:    
                c.draw(canvas, [pos[0], pos[1]])
                pos[0] += 77                                                                            
            
# define deck class 
class Deck:
    def __init__(self):
        # create a Deck object
        self.deck = []
        for suit in SUITS: 
            for rank in RANKS:
                # create a Card object using Card(suit, rank)
                # and add it to the card list for the deck    
                card_item = Card(suit, rank)
                self.deck.append(card_item)
                
    def shuffle(self):
        # shuffle the deck using random.shuffle()
        random.shuffle(self.deck)

    def deal_card(self):
        # deal a card object from the deck
        return self.deck.pop(0)
    
    def __str__(self):
        # return a string representing the deck
        str1 = ""
        for i in range (len(self.deck)):
            str1 = str1 + str(self.deck[i]) + " "                 
        return "Deck Contains " + str1


# define event handlers for buttons
def deal():
    global outcome, in_play, score, no_of_games, deck, player_hand, dealer_hand
    deck = Deck()
    deck.shuffle()
    
    player_hand = Hand()
    dealer_hand = Hand()
    
    player_hand.add_card(deck.deal_card())
    
    dealer_hand.add_card(deck.deal_card())
    player_hand.add_card(deck.deal_card())
    dealer_hand.add_card(deck.deal_card())
    
    outcome = "Hit or Stand?"
    if in_play:
        score -= 1
        outcome = "You lost! New round: Hit or Stand?"
    
    no_of_games += 1    
    in_play = True    
    
    
def hit():    
    global outcome, in_play, score    
    # if the hand is in play, hit the player
    if in_play:
        player_hand.add_card(deck.deal_card())
        if player_hand.get_value() <= 21:
            outcome = "Hit or Stand?"            
        # if busted, assign a message to outcome, update in_play and score
        else:
            in_play = False
            score -= 1
            outcome = "You have busted! New deal?"
    
def stand():
    global outcome, in_play, score   
    # if hand is in play, repeatedly hit dealer until his hand has value 17 or more
    if in_play == False:
        outcome = "You're busted!"
    else:
        while dealer_hand.get_value() < 17:
            dealer_hand.add_card(deck.deal_card())
        # assign a message to outcome, update in_play and score
        if dealer_hand.get_value() < player_hand.get_value():
            in_play = False
            score += 1
            outcome = "You won! New deal?"
        elif dealer_hand.get_value() >= player_hand.get_value():
            if dealer_hand.get_value() > 21:
                in_play = False
                score +=1
                outcome = "You won! New deal?"
            else:    
                in_play = False
                score -= 1
                outcome = "You have busted! New deal?"
                
            
# draw handler    
def draw(canvas):
    canvas.draw_text("Blackjack", [250, 30], 30, "Blue")
    
    canvas.draw_text("Player", [10, 85], 24, "Black")
    canvas.draw_text("Dealer", [10, 315], 24, "Black")
    canvas.draw_text("Score: " + str(score), [485, 30], 24, "Yellow")
    canvas.draw_text("Round: " + str(no_of_games), [485, 55], 21, "White")
    canvas.draw_text(outcome, [90, 575], 24, "White")
    
    player_hand.draw(canvas, [10, 100])
    dealer_hand.draw(canvas, [10, 330])

# initialization frame
frame = simplegui.create_frame("Blackjack", 600, 600)
frame.set_canvas_background("Green")

# create buttons and canvas callback
frame.add_button("Deal", deal, 200)
frame.add_button("Hit",  hit, 200)
frame.add_button("Stand", stand, 200)
frame.set_draw_handler(draw)


# get things rolling
deal()
frame.start()


# remember to review the gradic rubric
