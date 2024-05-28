DROP procedure IF EXISTS `v1_delete_user`;

DELIMITER $$
CREATE PROCEDURE `v1_delete_user` (IN vUserEmailId VARCHAR(36))
BEGIN
  UPDATE `users` SET IsActive = 0 WHERE EmailId = vUserEmailId;
END
$$

DELIMITER ;

