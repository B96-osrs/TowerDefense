from flask import Flask, jsonify
from flask_sqlalchemy import SQLAlchemy
from flask import request

app = Flask(__name__)

app.config['SQLALCHEMY_DATABASE_URI'] = 'postgresql://postgres:admin@localhost/tower_defense'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = False
db = SQLAlchemy(app)

# Model for the games table


class Games(db.Model):
    game_id = db.Column(db.Integer, primary_key=True, nullable=False)
    play_time_seconds = db.Column(db.Integer)
    coins_spent = db.Column(db.Integer)
    max_level_reached = db.Column(db.Integer)
    enemies_killed = db.Column(db.Integer)

    def __repr__(self):
        return f"Game {self.game_id}: Time: {self.play_time_seconds} min, Coins Spent: {self.coins_spent}, Max Wave: {self.max_wave_reached}, Enemies Killed: {self.enemies_killed}"


@app.route('/')
def hello_world():
    return 'Hello, World!'

# prints all the data in the database


@app.route('/list')
def fetch_data():
    fetch_result = Games.query.all()
    print(fetch_result)
    return jsonify([str(result) for result in fetch_result])

################################
# Add Data
################################


# add new game to the database
@app.route('/add_game', methods=['POST'])
def add_game():
    data = request.json

    play_time_seconds = data.get('play_time_seconds')
    coins_spent = data.get('coins_spent')
    max_level_reached = data.get('max_level_reached')
    enemies_killed = data.get('enemies_killed')

    new_game = Games(
        play_time_seconds=play_time_seconds,
        coins_spent=coins_spent,
        max_level_reached=max_level_reached,
        enemies_killed=enemies_killed
    )

    db.session.add(new_game)
    db.session.commit()
    return jsonify({"message": "Game added successfully!"}), 201


################################
# Get Data
################################

# returns amount of games played
@app.route('/games_played')
def fetch_games_played():
    games_played = db.session.query(db.func.count(Games.game_id)).scalar()
    return jsonify({'games_played': games_played})


# returns the max value for enemies_killed from the games table
@app.route('/max_enemies_killed')
def fetch_max_enemies_killed():
    max_enemies_killed = db.session.query(
        db.func.max(Games.enemies_killed)).scalar()
    return jsonify({'max_enemies_killed': max_enemies_killed})

# returns the average value for enemies_killed from the games table


@app.route('/avg_enemies_killed')
def fetch_avg_enemies_killed():
    avg_enemies_killed = db.session.query(
        db.func.avg(Games.enemies_killed)).scalar()
    return jsonify({'avg_enemies_killed': avg_enemies_killed})

# returns the max value for coins_spent from the games table


@app.route('/max_coins_spent')
def fetch_max_coins_spent():
    coins_spent = db.session.query(
        db.func.max(Games.coins_spent)).scalar()
    return jsonify({'max_coins_spent': coins_spent})

# returns the average value for coins_spent from the games table


@app.route('/avg_coins_spent')
def fetch_avg_coins_spent():
    coins_spent = db.session.query(
        db.func.avg(Games.coins_spent)).scalar()
    return jsonify({'avg_coins_spent': coins_spent})

# returns the highest level reached from the games table


@app.route('/max_level_reached')
def fetch_max_level():
    max_level = db.session.query(db.func.max(Games.max_level_reached)).scalar()
    return jsonify({'max_level_reached': max_level})

# returns the average level reached from the games table


@app.route('/avg_level_reached')
def fetch_avg_level():
    avg_level = db.session.query(db.func.avg(Games.max_level_reached)).scalar()
    return jsonify({'avg_level_reached': avg_level})


if __name__ == "__main__":
    app.run(debug=True)
