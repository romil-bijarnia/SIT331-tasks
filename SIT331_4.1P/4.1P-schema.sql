CREATE TABLE public.robotcommand (
    id integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    "Name" varchar(50) NOT NULL,
    description varchar(800),
    ismovecommand boolean NOT NULL,
    createddate timestamp NOT NULL,
    modifieddate timestamp NOT NULL
);
CREATE TABLE public.map (
    id integer GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    "Name" varchar(50) NOT NULL,
    rows integer NOT NULL,
    columns integer NOT NULL,
    issquare boolean GENERATED ALWAYS AS (rows > 0 AND rows = columns) STORED
);
INSERT INTO robotcommand ("Name", ismovecommand, createddate, modifieddate) VALUES
  ('LEFT',  true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
  ('RIGHT', true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
  ('UP',    true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP),
  ('DOWN',  true, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);
INSERT INTO map ("Name", rows, columns) VALUES ('DefaultMap', 5, 5);
