CREATE DATABASE LEETCODEDB;
USE LEETCODEDB;

CREATE TABLE Questions (
    id INT NOT NULL AUTO_INCREMENT,
    url VARCHAR(255) NOT NULL,
    name VARCHAR(200),
    qid INT,  # id on the web page
    PRIMARY KEY(id)
);

ALTER TABLE Questions ADD difficulty VARCHAR(20);

CREATE TABLE Metadata (
    id INT,
    priority INT,
    last_touch DATE,
    note TEXT,
    PRIMARY KEY(id),
    FOREIGN KEY(id) REFERENCES Questions(id)
);

CREATE TABLE Company (
    id INT,
    company VARCHAR(100),
    PRIMARY KEY(id, company),
    FOREIGN KEY(id) REFERENCES Questions(id)
);

CREATE TABLE Tag (
    id INT,
    tag VARCHAR(100),
    PRIMARY KEY(id, tag),
    FOREIGN KEY(id) REFERENCES Questions(id)
);

CREATE TABLE SimilarRel (
    low_id INT,
    high_id INT,
    PRIMARY KEY(low_id, high_id),
    FOREIGN KEY(low_id) REFERENCES Questions(id),
    FOREIGN KEY(high_id) REFERENCES Questions(id)
);


