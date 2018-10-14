# AWS

## Access ways

1. AWS Management Console: A web interface.
2. AWS Command Line Interface (AWS CLI): Commands for a broad set of AWS products.
3. Command Line Tools: Commands for individual AWS products.
4. AWS Software Development Kits (SDK): APIs that are specific to your programming language or platform.
5. Query APIs: Low-level APIs that you access using HTTP requests.
6. Getting Started with AWS

## Region code: data centers

- US East (N. Virginia): us-east-1
- US West (N. California): us-west-1
- US West (Oregon): us-west-2

## Security

- Identity and Access Management (IAM): create fine-grained permissions to AWS resources and apply them to users or groups of users.
- ACL-type permissions: on data and can also use encryption of data at rest.
- Virtual private cloud (VPC): a virtual network that is logically isolated from other virtual networks in the AWS cloud. Can control whether the network is directly routable to the Internet.
- Set up a security group: acts as a virtual firewall to control the inbound and outbound traffic for your virtual servers.
- Specify a key pair when launch virtual server: encrypt the login information. Must present the private key of the key pair to decrypt the login information.

## AWS Product Categories

- Compute and Networking Services
- Storage and Content Delivery Services
- Database Services
- Analytics Services
- App Services
- Deployment Services
- Management Services
- AWS Overview

## Practice

- Run a Virtual Server: Launch a virtual server using Amazon EC2 for complete control of your AWS computing resources.
- Store Files: Use Amazon S3 to store data and retrieve it quickly. Use Amazon Glacier to store archival data at a low cost.
- Deploy a Website: You can use AWS to host your static website. You can also use AWS deployment services to quickly set up a dynamic website. Deploy a static website using Amazon S3. Deploy a web app using AWS Elastic Beanstalk.
- Host a Website: Hosting a WordPress Blog with Amazon EC2. Host Drupal using scaling and load balancing. Host a .NET app using scaling and load balancing.
- Run a Database: Use Amazon DynamoDB, a fully-managed NoSQL database service. Use Amazon RDS to set up, operate, and scale a relational database in the cloud. Use Amazon Redshift, a fast, fully-managed, petabyte-scale data warehouse solution.
- Analyze Your Data: Use Amazon EMR to count words. Use Hive with Amazon EMR to query Apache web server logs. Use Hadoop with Amazon EMR to evaluate Twitter data. Automate moving and processing data using AWS Data Pipeline.

## AWS CLI

Check if python version is higher than 2.6.5.

Go to <https://pip.pypa.io/en/latest/installing.html>, download get-pip.py.

```bash
$ python get-pip.py # --user to install.
2.7.8

$ sudo apt-get install python-pip.
$ pip install awscli
```

Configure

```bash
$ aws configure
AWS Access Key ID [None]: AKIAIOSFODNN7EXAMPLE
AWS Secret Access Key [None]: wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY
Default region name [None]: us-west-1
Default output format [None]: json
http://aws.amazon.com/cli/
```

## EC2

Amazon Elastic Compute Cloud (Amazon EC2): The instance is an Amazon EBS-backed instance. Need select available zones.

- Create access keys for AWS account to access the command line interface or API.
- Create an IAM User: Can also create an IAM user, and then add the user to an IAM group with administrative permissions or grant this user administrative permissions. Then access AWS using a special URL and the credentials for the IAM user.

<https://console.aws.amazon.com/iam/?#home>

1. Create a group names Administrators.
2. Select template: Administrator Access.
3. Create new users.
4. Download Credential.
5. Check the user under Users and then navigate to Groups, click the group and add the user.
6. In this user's information page, go down to Security Credential, Manage Password. MyPass+Z1
7. Sign out AWS, and use <https://your_aws_account_id.signin.aws.amazon.com/console/> to log in. Account ID is a serial numbers in 1234-5678-9012 format and AWS account ID is 123456789012.
8. Log in at: <https://123456789012.signin.aws.amazon.com/console/> Can Create Account Alias to change id part.

- Create a key pair: key pairs are specific to the region. A Linux instance has no password; you use a key pair to log in to your instance securely. You specify the name of the key pair when you launch your instance, then provide the private key when you log in using SSH.
1. Under EC2 console, Key pairs. Name is key-pair-uswest2. Private key file is *.pem. Need to provide the name of your key pair when you launch an instance and the corresponding private key each time you connect to the instance.
2. chmod 400 key-pair-uswest2.pem
3. Configue SSH.

- Create a Virtual Private Cloud (VPC): Amazon VPC enables you to launch AWS resources into a virtual network that you've defined.
1. <https://console.aws.amazon.com/vpc/>.
2. Select a region.
3. Start VPC Wizard.
4. Select VPC with a Single Public Subset.
5. Give a name. MyVPC.

- Create a Security Group: act as a firewall for associated instances, controlling both inbound and outbound traffic at the instance level. you'll need to create a security group in each region.
1. Get the IP of local PC: 160.39.200.62.
2. Choose a region.
3. Security Groups: Create Security Group, name: zhenying_SG_uswest2. Description and select VPC.
4. Inbound Rules: HTTP and HTTPS, 0.0.0.0/0.
5. SSH: local IP/32 to specify an individual IP address in CIDR notation.

- Amazon Machine Images (AMIs): serve as templates for your instance.
1. EC2 Console: Launch Instance.
2. Choose an AMI.
3. Choose hardware configuration. Then Next: Configure Instance Details.
4. Select an VPC to access t2 instance. Enable the auto assign public IP.
5. Review and Launch to create the instance.
6. Edit Security Groups: Select an existing security group.
7. Launch.
8. Choose an existing key pair.
9. Wait until initialize.

- If you're using a public AMI from a third party, you can use the command line tools to verify the fingerprint by installing the AWS CLI or Amazon EC2 CLI.
1. chmod 400 key-pair-uswest2.pem
2. Get public IP.
3. `ssh -i key-pair-uswest2.pem ec2-user@54.148.111.222`
4. yes to check fingerprint.

- Transfer file:
1. install SCP if not come with ssh.
2. `scp -i key-pair-uswest2.pem SampleFile.txt ec2-user@ec2-54-148-111-22.us-west-2.compute.amazonaws.com:~`

- Actions: Terminate means delete the instance. Stop can restart. Volume can delete.

- Works on the default VPC
1. Connect to Amazon Linux instance.
2. `sudo yum update -y`   y means don't show processing
3. `sudo yum groupinstall -y “Web Server” “MySQL Database” “PHP Support”`
4. `sudo yum install -y php-mysql`
5. `sudo service httpd start`
6. `sudo chkconfig httpd on` to let httpd start every boot and check if the server start    chkconfig --list httpd   the number is the runlevels. 
7. Use the public ip to see the test page.
8. `sudo groupadd www`
9. `sudo usermod -a -G www ec2-user`
10. Disconnect and log in again. groups to check.
11. `sudo chown -R root:www /var/www`
12. `sudo chmod 2775 /var/www` and `find /var/www -type d -exec sudo chmod 2775 {} +` and `find /var/www -type f -exec sudo chmod 0664 {} +`
13. Add contents to /var/www/: `echo "<?php phpinfo(); ?>" > /var/www/html/phpinfo.php`
14. `sudo service mysqld start`
15. `sudo mysql_secure_installation`   to start configuration. Root has no password so enter. Press yes and follow instruction. 
16. `mysql -u root -p`
17. `CREATE USER 'newuser'@'localhost' IDENTIFIED BY 'PASSWORD'; GRANT ALL PRIVILEGES ON * . * TO 'newuser'@'localhost'; FLUSH PRIVILEGES;`
18. CREATE DATABASE my_db;
19. login as user and `USE my_db;`

Tutorial: Installing a LAMP Web Server

1. Tomcat: `wget http://www.webhostingjams.com/mirror/apache/tomcat/tomcat-7/v7.0.57/bin/apache-tomcat-7.0.57.tar.gz`
2. sudo -i and tar the file. 
3. Create tomcat in /etc/init.d to let it start each time restart the instance.

```bash
!/bin/sh
# Tomcat init script for Linux.
#
# chkconfig: 2345 96 14
# description: The Apache Tomcat servlet/JSP container.
CATALINA_HOME=/usr/share/apache-tomcat-7.0.57
export CATALINA_HOME
exec $CATALINA_HOME/bin/catalina.sh $*
```

4. Start it: chmod 755 /etc/rc.d/init.d/tomcat    chkconfig --level 2345 tomcat on
5. Create user by modify apache-tomcat-7.0.42/config/tomcat-user.xml

```xml
<role rolename="manager-gui"/>
<role rolename="manager-script"/>
<role rolename="manager-jmx"/>
<role rolename="manager-status"/>
<role rolename="admin-gui"/>

<user username="tomcat" password="password" roles="manager-gui,manager-status,admin-gui"/>
<user username="tomcattools" password="password" roles="manager-jmx,manager-script"/>
```

6. Reboot the instance. Check the 8080 port. Click the Manage App and enter the password.

Amazon Free Usage Tier: Installing Tomcat 7 on an EC2 Linux instance

## S3
s3cmd is a community tool written in python. 
```
s3cmd --configure
    Access Key and Secret Key: In credentials.csv. 
    Encryption password: A password for encrypt. S+Z1
    GPG: just press Enter. 
    HTTPs and other setting: Don't need ssh. 
    After setting, a s3cfg file will be saved. 
```
http://kb.site5.com/shell-access-ssh/how-to-setup-and-configure-the-s3cmd-tool-for-amazon-s3/

##
Get metadata from instance
```
curl -f http://169.254.169.254/latest/meta-data/public-keys/0/openssh-key  > /root/.ssh/authorized_keys
```
