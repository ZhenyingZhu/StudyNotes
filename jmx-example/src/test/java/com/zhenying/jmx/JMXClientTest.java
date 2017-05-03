package com.zhenying.jmx;

import org.junit.Before;
import org.junit.Test;

import javax.management.AttributeChangeNotification;
import javax.management.JMX;
import javax.management.MBeanServerConnection;
import javax.management.Notification;
import javax.management.NotificationBroadcaster;
import javax.management.NotificationListener;
import javax.management.ObjectName;
import javax.management.remote.JMXConnector;
import javax.management.remote.JMXConnectorFactory;
import javax.management.remote.JMXServiceURL;

import java.util.Arrays;

/**
 * Created by zhenyinz on 5/1/17.
 */

public class JMXClientTest {
    private SystemConfigMBean mBean;

    @Before
    public void setupJMXClient() throws Exception {
        JMXServiceURL url = new JMXServiceURL("service:jmx:rmi:///jndi/rmi://:90/jmxrmi");
        JMXConnector jmxc = JMXConnectorFactory.connect(url, null);

        MBeanServerConnection mbsc = jmxc.getMBeanServerConnection();

        ObjectName mbeanName = new ObjectName("com.zhenying.jmx:type=SystemConfig");
        mBean = JMX.newMBeanProxy(mbsc, mbeanName, SystemConfigMBean.class, true);
    }

    @Test
    public void test() throws InterruptedException {
        System.out.println(mBean.getSchemaName());
        mBean.setSchemaName("Unit Test");
        System.out.println(mBean.getSchemaName());

        System.out.println(mBean.getThreadCount());
        Thread.sleep(1000);
        System.out.println(mBean.getThreadCount());
    }
}
