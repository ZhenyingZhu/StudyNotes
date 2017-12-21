<?php 
ini_set('display_errors', 'On'); 
$db = "w4111b.cs.columbia.edu:1521/adb";
$conn=oci_connect("zz2283", "Columbia2014", $db);
$stmt = oci_parse($conn, "SELECT sname FROM Subjects");
oci_execute($stmt, OCI_DEFAULT);
while ($res=oci_fetch_row($stmt))
{
	echo "Table Name: ". $res[0] . "<br />"; 
}
oci_close($conn);
?>
