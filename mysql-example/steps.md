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
ALTER TABLE Questions ADD UNIQUE (url);

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
    company VARCHAR(100),  # need subscription
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


SELECT DISTINCT difficulty from Questions;
SELECT * from Metadata ORDER BY last_touch DESC;
SELECT * FROM Questions, Metadata WHERE Questions.id = Metadata.id AND Questions.url="https://leetcode.com/problems/two-sum/";
SELECT * FROM Questions, Tag WHERE Questions.id = Tag.id AND Questions.url="https://leetcode.com/problems/add-two-numbers/";
SELECT * FROM Questions LEFT JOIN Tag ON Questions.id = Tag.id WHERE Questions.url="https://leetcode.com/problems/two-sum/"; # no tag


