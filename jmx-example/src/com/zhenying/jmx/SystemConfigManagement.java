package com.zhenying.jmx;

import javax.management.InstanceAlreadyExistsException;
import javax.management.MBeanRegistrationException;
import javax.management.MBeanServer;
import javax.management.MalformedObjectNameException;
import javax.management.NotCompliantMBeanException;
import javax.management.ObjectName;
import java.lang.management.ManagementFactory;

/**
 * Created by zhenyinz on 4/26/17.
 */
public class SystemConfigManagement {
    private static final int DEFAULT_NO_THREADS = 10;
    private static final String DEFAULT_SCHEMA = "default";

    public static void main(String[] args) throws
            MalformedObjectNameException, NotCompliantMBeanException, InstanceAlreadyExistsException,
            MBeanRegistrationException, InterruptedException {
        MBeanServer mbs = ManagementFactory.getPlatformMBeanServer();

        SystemConfig mBean = new SystemConfig(DEFAULT_NO_THREADS, DEFAULT_SCHEMA);

        ObjectName name = new ObjectName("com.zhenying.jmx:type=SystemConfigTest");

        mbs.registerMBean(mBean, name);

        do {
            Thread.sleep(3000);
            System.out.println( "Thread Count="+mBean.getThreadCount()+":::Schema Name="+mBean.getSchemaName() );
        } while (mBean.getThreadCount() != 0);
    }
}
