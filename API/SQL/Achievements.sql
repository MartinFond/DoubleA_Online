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