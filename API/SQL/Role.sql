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