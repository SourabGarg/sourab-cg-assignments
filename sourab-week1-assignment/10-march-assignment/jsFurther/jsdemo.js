function test() {
    console.log("Hello world");
}

function test2(num1, num2) {
    return (num1 + num2);
}

test();
let sum = test2(12, 45);
console.log("Sum:", sum);

// ============================================
// MAP FUNCTION EXAMPLES
// ============================================

console.log("\n--- MAP FUNCTION EXAMPLES ---\n");

// Example 1: Basic map - square each number
const numbers = [1, 2, 3, 4, 5];
const squared = numbers.map(num => num * num);
console.log("1. Original numbers:", numbers);
console.log("   Squared numbers:", squared);

// Example 2: Map with index parameter
const numbersWithIndex = [10, 20, 30, 40];
const withIndex = numbersWithIndex.map((num, index) => `Index ${index}: ${num}`);
console.log("\n2. Numbers with index:", withIndex);

// Example 3: Converting to different data type
const prices = [10, 25, 50, 75];
const priceStrings = prices.map(price => `$${price.toFixed(2)}`);
console.log("\n3. Price strings:", priceStrings);

// Example 4: Working with objects
const users = [
    { id: 1, name: "Alice", age: 25 },
    { id: 2, name: "Bob", age: 30 },
    { id: 3, name: "Charlie", age: 35 }
];
const userNames = users.map(user => user.name);
console.log("\n4. User names:", userNames);

// Example 5: Adding properties to objects
const usersWithEmail = users.map(user => ({
    ...user,
    email: `${user.name.toLowerCase()}@example.com`
}));
console.log("\n5. Users with email:", usersWithEmail);

// Example 6: Map with full function syntax
const temperatures = [0, 10, 20, 30, 40];
const fahrenheit = temperatures.map(function(celsius) {
    return (celsius * 9/5) + 32;
});
console.log("\n6. Celsius to Fahrenheit:");
console.log("   Celsius:", temperatures);
console.log("   Fahrenheit:", fahrenheit);

// Example 7: Chaining map with other array methods
const mixedNumbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
const result = mixedNumbers
    .filter(num => num % 2 === 0)  // Get even numbers
    .map(num => num * 3)           // Multiply by 3
    .map(num => `Result: ${num}`); // Format as string
console.log("\n7. Chained operations (even * 3):", result);

// Example 8: Map with Math operations
const values = [1, 4, 9, 16, 25];
const squareRoots = values.map(Math.sqrt);
console.log("\n8. Square roots:", squareRoots);

// Example 9: Extracting specific properties from complex objects
const products = [
    { id: 1, name: "Laptop", price: 999, inStock: true },
    { id: 2, name: "Mouse", price: 29, inStock: false },
    { id: 3, name: "Keyboard", price: 79, inStock: true }
];
const productInfo = products.map(p => ({
    name: p.name,
    available: p.inStock ? "In Stock" : "Out of Stock"
}));
console.log("\n9. Product availability:", productInfo);

// Example 10: Using map with array of arrays
const matrix = [[1, 2], [3, 4], [5, 6]];
const sumOfPairs = matrix.map(pair => pair[0] + pair[1]);
console.log("\n10. Sum of pairs from matrix:", sumOfPairs);

// Example 11: Map returning different structures
const students = ["Alice", "Bob", "Charlie"];
const studentObjects = students.map((name, index) => ({
    id: index + 1,
    name: name,
    enrolled: true
}));
console.log("\n11. Student objects:", studentObjects);

// Example 12: Map with conditional logic
const grades = [45, 78, 92, 65, 88];
const gradeStatus = grades.map(grade => {
    if (grade >= 90) return { grade, status: "Excellent" };
    if (grade >= 70) return { grade, status: "Good" };
    if (grade >= 50) return { grade, status: "Pass" };
    return { grade, status: "Fail" };
});
console.log("\n12. Grade status:", gradeStatus);

// Example 13: Map vs forEach (map returns new array)
const original = [1, 2, 3];
const mapped = original.map(x => x * 2);
console.log("\n13. Map returns new array:");
console.log("    Original:", original);
console.log("    Mapped:", mapped);

// Example 14: Map with string manipulation
const words = ["hello", "world", "javascript", "map"];
const capitalized = words.map(word => word.toUpperCase());
const lengths = words.map(word => word.length);
console.log("\n14. String manipulation:");
console.log("    Capitalized:", capitalized);
console.log("    Lengths:", lengths);

// Example 15: Practical example - calculating totals
const cart = [
    { item: "Apple", price: 2, quantity: 3 },
    { item: "Banana", price: 1, quantity: 5 },
    { item: "Orange", price: 3, quantity: 2 }
];
const itemTotals = cart.map(item => ({
    item: item.item,
    total: item.price * item.quantity
}));
console.log("\n15. Shopping cart totals:", itemTotals);
