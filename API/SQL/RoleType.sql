-- Type: role_type

-- DROP TYPE IF EXISTS public.role_type;

CREATE TYPE public.role_type AS ENUM
    ('none', 'player', 'dgs');

ALTER TYPE public.role_type
    OWNER TO martin;