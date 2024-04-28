-- Type: rank

-- DROP TYPE IF EXISTS public.rank;

CREATE TYPE public.rank AS ENUM
    ('unranked', 'bronze', 'argent', 'or', 'platine', 'diamant');

ALTER TYPE public.rank
    OWNER TO martin;
