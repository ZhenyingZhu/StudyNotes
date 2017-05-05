import java.lang.reflect.Constructor;
import java.lang.reflect.InvocationTargetException;
import java.util.Arrays;
import java.util.EnumMap;
import java.util.Map;

/**
 * Should use builder 
 */
public class QueryFactory {
    private enum QueryType {
        TREE
    };

    private static Map<QueryType, Class<? extends AbstractQuery>> enumMap;

    static {
        enumMap = new EnumMap<>(QueryType.class);
        enumMap.put(QueryType.TREE, TreeQuery.class);
    }

    public String getAllTypes() {
        return Arrays.toString(QueryType.values());
    }

    public AbstractQuery getQuery(String typeStr, String dropletIp, String side)
            throws NoSuchMethodException, IllegalAccessException, InvocationTargetException, InstantiationException {
        if (enumMap.containsKey(QueryType.valueOf(typeStr))) {
            Constructor<?> constructor = enumMap.get(QueryType.valueOf(typeStr)).getConstructor(String.class);
            return (AbstractQuery) constructor.newInstance(dropletIp, side);
        } else {
            throw new IllegalArgumentException("Unsupported type");
        }
    }
}
