CREATE TABLE public."user" (
    id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    email VARCHAR(100) NOT NULL,
    firstname VARCHAR(50) NOT NULL,
    lastname VARCHAR(50) NOT NULL,
    passwordhash VARCHAR(200) NOT NULL,
    description VARCHAR(500),
    role VARCHAR(50),
    createddate TIMESTAMP NOT NULL,
    modifieddate TIMESTAMP NOT NULL
);
SELECT * FROM "user";