package com.zhenying.jmx;

/**
 * Created by zhenyinz on 4/26/17.
 */
public class SystemConfig implements SystemConfigMBean {
    private int threadCount;
    private String schemaName;

    public SystemConfig(int noOfThreads, String schema) {
        threadCount = noOfThreads;
        schemaName = schema;
    }

    @Override
    public void setThreadCount(int noOfThreads) {
        threadCount = noOfThreads;
    }

    @Override
    public int getThreadCount() {
        return threadCount;
    }

    @Override
    public void setSchemaName(String schemaName) {
        this.schemaName = schemaName;
    }

    @Override
    public String getSchemaName() {
        return schemaName;
    }

    @Override
    public String doConfig() {
        return "NoOfThreads=" + threadCount + " DB SchemaName=" + schemaName;
    }
}
