CREATE SCHEMA `taggedfs` DEFAULT CHARACTER SET utf8 ;

CREATE TABLE `taggedfs`.`files` (
  `id` INT(7) UNSIGNED ZEROFILL NOT NULL AUTO_INCREMENT,
  `path` VARCHAR(260) NOT NULL,
  `description` LONGTEXT NULL,
  `name` VARCHAR(45) NOT NULL,
  `hardcopy` VARCHAR(45) NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

ALTER TABLE `taggedfs`.`files` 
RENAME TO  `taggedfs`.`file` ;

ALTER TABLE `taggedfs`.`file` 
CHANGE COLUMN `path` `path` VARCHAR(260) NULL ;

CREATE TABLE `taggedfs`.`tag` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `name_UNIQUE` (`name` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE TABLE `taggedfs`.`tagrel` (
  `parent` INT NOT NULL,
  `child` INT NOT NULL,
  PRIMARY KEY (`parent`, `child`),
  INDEX `TagRel_Child_idx` (`child` ASC),
  CONSTRAINT `TagRel_Parent`
    FOREIGN KEY (`parent`)
    REFERENCES `taggedfs`.`tag` (`id`)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION,
  CONSTRAINT `TagRel_Child`
    FOREIGN KEY (`child`)
    REFERENCES `taggedfs`.`tag` (`id`)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION);

CREATE TABLE `taggedfs`.`filetag` (
  `file` INT(7) UNSIGNED NOT NULL,
  `tag` INT(11) NOT NULL,
  PRIMARY KEY (`file`, `tag`),
  INDEX `FileTag_Tag_idx` (`tag` ASC),
  CONSTRAINT `FileTag_Tag`
    FOREIGN KEY (`tag`)
    REFERENCES `taggedfs`.`tag` (`id`)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION,
  CONSTRAINT `FileTag_File`
    FOREIGN KEY (`file`)
    REFERENCES `taggedfs`.`file` (`id`)
    ON DELETE RESTRICT
    ON UPDATE NO ACTION);
