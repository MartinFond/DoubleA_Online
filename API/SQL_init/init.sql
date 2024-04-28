CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

GRANT ALL PRIVILEGES ON DATABASE my_api_rest TO martin;

\c my_api_rest

-- Type: role_type

-- DROP TYPE IF EXISTS public.role_type;

CREATE TYPE public.role_type AS ENUM
    ('none', 'player', 'dgs');

ALTER TYPE public.role_type
    OWNER TO martin;

-- Type: rank

-- DROP TYPE IF EXISTS public.rank;

CREATE TYPE public.rank AS ENUM
    ('unranked', 'bronze', 'argent', 'or', 'platine', 'diamant');

ALTER TYPE public.rank
    OWNER TO martin;


-- Table: public.role

-- DROP TABLE IF EXISTS public.role;

CREATE TABLE IF NOT EXISTS public.role
(
    id role_type,
    CONSTRAINT unique_role_id UNIQUE (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.role
    OWNER to martin;


-- Table: public.User

-- DROP TABLE IF EXISTS public."User";

CREATE TABLE IF NOT EXISTS public."User"
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    username character varying(255) COLLATE pg_catalog."default" NOT NULL,
    email character varying(255) COLLATE pg_catalog."default" NOT NULL,
    password character varying(255) COLLATE pg_catalog."default" NOT NULL,
    salt character varying(255) COLLATE pg_catalog."default" NOT NULL,
    role role_type,
    rank rank NOT NULL DEFAULT 'unranked'::rank,
    CONSTRAINT "User_pkey" PRIMARY KEY (id),
    CONSTRAINT "User_email_key" UNIQUE (email),
    CONSTRAINT "User_username_key" UNIQUE (username),
    CONSTRAINT fk_user_role FOREIGN KEY (role)
        REFERENCES public.role (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."User"
    OWNER to martin;


-- Table: public.Achievements

-- DROP TABLE IF EXISTS public."Achievements";

CREATE TABLE IF NOT EXISTS public."Achievements"
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    name character varying(255) COLLATE pg_catalog."default" NOT NULL,
    description text COLLATE pg_catalog."default",
    image_url character varying(255) COLLATE pg_catalog."default",
    CONSTRAINT "Achievements_pkey" PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Achievements"
    OWNER to martin;


-- Table: public.UserAchievements

-- DROP TABLE IF EXISTS public."UserAchievements";

CREATE TABLE IF NOT EXISTS public."UserAchievements"
(
    user_id uuid NOT NULL,
    achievement_id uuid NOT NULL,
    CONSTRAINT "UserAchievements_pk" PRIMARY KEY (user_id, achievement_id),
    CONSTRAINT "UserAchievements_achievement_id_fk" FOREIGN KEY (achievement_id)
        REFERENCES public."Achievements" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    CONSTRAINT "UserAchievements_user_id_fk" FOREIGN KEY (user_id)
        REFERENCES public."User" (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."UserAchievements"
    OWNER to martin;




-- Values initialisation

INSERT INTO public.role (id) VALUES ('player');
INSERT INTO public.role (id) VALUES ('dgs');

INSERT INTO public."Achievements" ( name, description, image_url)
VALUES ('exempleAchievement', 'achievement_description', 'imageEx_url');
