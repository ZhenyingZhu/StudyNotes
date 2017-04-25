interface Names {
    public void sayName(String name);
}

class Lambda {
    private static void notUseLambda() {
        // boilerplate code
        Names nameInstance = new Names() {
            @Override
            public void sayName(String name) {
                System.out.println("Hello " + name);
            }
        };

        myName(nameInstance, "John");
    }

    private static void useLambda() {
        myName(n -> System.out.println("Lambda Hello " + n), "John");
    }

    // function to call as Lambda express
    private static void myName(Names nameInstance, String name) {
        nameInstance.sayName(name);
    }

    public static void main(String[] args) {
        useLambda();
    }
}
