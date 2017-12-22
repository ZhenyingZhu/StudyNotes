<?php
// This is trying to see if using multiple forms can seperate the post of radio box. 
// If using one form, what ever the button name is, the form post every radio box value. 
for($i = 0; $i < 5; $i ++) {
	if (isset ( $_POST ["post_" . $i] )) {
		$post_i = $_POST ["post_" . $i];
		$position_i=$i;
		$echo .= $post_i;
	}
}
?>
<html>
<body>
<?php
for($j = 0; $j < 5; $j ++) {
	echo "<form action=\"tryDupPostForms.php\" method=\"post\">";
	echo "<table><tr><td>This is " . $j . "</td>";
	for($rate_level = 1; $rate_level < 6; $rate_level ++) {
		echo "<td><input type=\"radio\" name=\"post_" . $j . "\" value=\"" . $rate_level . "\" /></td> ";
	}
	echo "<td>position ".$position_i." rating=".$post_i."</td>";
	echo "<td><input type=\"submit\" value=\"change\" /></td></tr></table></form>";
}

?>
</body>
</html>