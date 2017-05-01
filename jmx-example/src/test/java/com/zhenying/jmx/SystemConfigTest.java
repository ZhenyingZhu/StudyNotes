package com.zhenying.jmx;


import org.junit.Before;
import org.junit.Test;

/**
 * Created by zhenyinz on 5/1/17.
 * Just try Junit
 */
public class SystemConfigTest {
    private SystemConfig sut;

    @Before
    public void setup() {
        System.out.println("Start test");
        sut = new SystemConfig(1, "Test");
    }

    @Test
    public void test() {
        assert(sut.getThreadCount() == 1);
    }
}