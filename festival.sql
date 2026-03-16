-- Table: artists
CREATE TABLE IF NOT EXISTS artists (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL
);

-- Table: shows
CREATE TABLE IF NOT EXISTS shows (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    artist_id INTEGER NOT NULL,
    show_date TEXT NOT NULL,
    show_time TEXT NOT NULL,
    location TEXT NOT NULL,
    available_seats INTEGER NOT NULL,
    sold_seats INTEGER NOT NULL,
    FOREIGN KEY (artist_id) REFERENCES artists (id) ON DELETE CASCADE
);

-- Table: employees
CREATE TABLE IF NOT EXISTS employees (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    username TEXT UNIQUE NOT NULL,
    password TEXT NOT NULL
);

-- Table: tickets
CREATE TABLE IF NOT EXISTS tickets (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    show_id INTEGER NOT NULL,
    buyer_name TEXT NOT NULL,
    number_of_seats INTEGER NOT NULL,
    FOREIGN KEY (show_id) REFERENCES shows (id) ON DELETE CASCADE
);

-- Initial Data for Testing
INSERT INTO artists (name) VALUES ('Caty'), ('Max'), ('Lily');
INSERT INTO employees (username, password) VALUES ('admin', 'admin123');