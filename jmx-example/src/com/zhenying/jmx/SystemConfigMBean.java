package com.zhenying.jmx;

/**
 * Created by zhenyinz on 4/26/17.
 */
public interface SystemConfigMBean {
    public void setThreadCount(int noOfThreads);
    public int getThreadCount();

    public void setSchemaName(String schemaName);
    public String getSchemaName();

    public String doConfig();
}
