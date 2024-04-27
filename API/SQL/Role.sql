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