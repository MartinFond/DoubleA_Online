UPDATE public."role"
SET role_type = CASE id
    WHEN 1 THEN 'Player'::roletype
    WHEN 2 THEN 'DGS'::roletype
    -- Add more cases for other id values if needed
    ELSE NULL -- Handle any unexpected values
END;

ALTER TYPE role_type RENAME VALUE 'DGS' TO 'dgs';