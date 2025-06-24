using CoursePlatform.Domain.Entities;
using CoursePlatform.Domain.Repositories;
using CoursePlatform.Infrastructure.Persistence.Repositories;

namespace CoursePlatform.Infrastructure.Examples;

/// <summary>
/// This class demonstrates how to use the Generic Repository pattern in your CoursePlatform application.
/// The Generic Repository pattern provides common CRUD operations for all entities that inherit from BaseEntity,
/// while still allowing for entity-specific methods in specialized repositories.
/// </summary>
public class GenericRepositoryUsageExample
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Student> _genericStudentRepo;
    private readonly IStudentRepository _specificStudentRepo;

    public GenericRepositoryUsageExample(IUnitOfWork unitOfWork, IGenericRepository<Student> genericStudentRepo, IStudentRepository specificStudentRepo)
    {
        _unitOfWork = unitOfWork;
        _genericStudentRepo = genericStudentRepo;
        _specificStudentRepo = specificStudentRepo;
    }

    /// <summary>
    /// Demonstrates basic CRUD operations using the generic repository
    /// </summary>
    public async Task BasicCrudOperationsExample()
    {
        // 1. CREATE - Add a new student using generic repository
        var newStudent = new Student
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        await _genericStudentRepo.AddAsync(newStudent);
        await _unitOfWork.SaveChangesAsync();

        // 2. READ - Get by ID using generic repository
        var student = await _genericStudentRepo.GetByIdAsync(newStudent.Id);
        
        // 3. READ - Get all students using generic repository
        var allStudents = await _genericStudentRepo.GetAllAsync();

        // 4. UPDATE - Update using generic repository
        if (student != null)
        {
            student.Email = "john.doe.updated@example.com";
            await _genericStudentRepo.UpdateAsync(student);
            await _unitOfWork.SaveChangesAsync();
        }

        // 5. DELETE - Delete using generic repository
        if (student != null)
        {
            await _genericStudentRepo.DeleteAsync(student);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Demonstrates advanced querying capabilities of the generic repository
    /// </summary>
    public async Task AdvancedQueryingExample()
    {
        // 1. CONDITIONAL QUERIES - Find students with specific criteria
        var studentsWithGmail = await _genericStudentRepo.GetWhereAsync(s => s.Email.Contains("@gmail.com"));
        
        // 2. SINGLE RESULT QUERIES - Find first student matching criteria
        var firstStudent = await _genericStudentRepo.GetFirstOrDefaultAsync(s => s.FirstName == "John");

        // 3. EXISTENCE CHECKS - Check if any students match criteria
        var hasStudents = await _genericStudentRepo.AnyAsync(s => s.LastName == "Smith");

        // 4. COUNTING - Count total students
        var totalStudents = await _genericStudentRepo.CountAsync();
        
        // 5. CONDITIONAL COUNTING - Count students with criteria
        var gmailUsersCount = await _genericStudentRepo.CountAsync(s => s.Email.Contains("@gmail.com"));

        // 6. INCLUDING RELATED DATA - Get students with their enrollments
        var studentsWithEnrollments = await _genericStudentRepo.GetAllWithIncludesAsync(s => s.Enrollments);
        
        // 7. COMPLEX INCLUDES - Get student by ID with multiple includes
        var studentWithDetails = await _genericStudentRepo.GetByIdWithIncludesAsync(
            "student-id", 
            s => s.Enrollments, 
            s => s.Enrollments.Select(e => e.Course)
        );
    }

    /// <summary>
    /// Demonstrates when to use specific repository methods vs generic repository methods
    /// </summary>
    public async Task SpecificVsGenericRepositoryExample()
    {
        // USE GENERIC REPOSITORY FOR:
        // - Basic CRUD operations
        // - Simple queries with Where, Count, Any, etc.
        // - Operations that are common across all entities

        // Basic operations using generic repository
        var allStudents = await _genericStudentRepo.GetAllAsync();
        var activeStudents = await _genericStudentRepo.GetWhereAsync(s => s.Email != null);
        var studentCount = await _genericStudentRepo.CountAsync();

        // USE SPECIFIC REPOSITORY FOR:
        // - Domain-specific business logic
        // - Complex queries with specific includes
        // - Operations that are unique to the entity

        // Domain-specific operations using specific repository
        var studentByEmail = await _specificStudentRepo.GetByEmailAsync("john@example.com");
        var studentWithEnrollments = await _specificStudentRepo.GetByIdWithEnrollmentsAsync("student-id");
        var recentlyActiveStudents = await _specificStudentRepo.GetActiveStudentsAsync();
    }

    /// <summary>
    /// Demonstrates transaction usage with repositories
    /// </summary>
    public async Task TransactionExample()
    {
        try
        {
            // Begin transaction
            await _unitOfWork.BeginTransactionAsync();

            // Perform multiple operations
            var student1 = new Student { FirstName = "Alice", LastName = "Johnson", Email = "alice@example.com" };
            var student2 = new Student { FirstName = "Bob", LastName = "Wilson", Email = "bob@example.com" };

            await _genericStudentRepo.AddAsync(student1);
            await _genericStudentRepo.AddAsync(student2);

            // Save changes within transaction
            await _unitOfWork.SaveChangesAsync();

            // Commit transaction
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception)
        {
            // Rollback transaction on error
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    /// <summary>
    /// Demonstrates working with different entity types using their specific repositories
    /// </summary>
    public async Task MultipleEntitiesExample()
    {
        // Working with courses
        var allCourses = await _unitOfWork.Courses.GetAllAsync(); // Uses generic methods
        var courseWithInstructor = await _unitOfWork.Courses.GetByIdWithInstructorAsync("course-id"); // Uses specific method

        // Working with instructors
        var allInstructors = await _unitOfWork.Instructors.GetAllAsync(); // Uses generic methods
        var instructorWithCourses = await _unitOfWork.Instructors.GetByIdWithCoursesAsync("instructor-id"); // Uses specific method

        // Working with enrollments
        var allEnrollments = await _unitOfWork.Enrollments.GetAllAsync(); // Uses generic methods
        var studentEnrollments = await _unitOfWork.Enrollments.GetEnrollmentsByStudentAsync("student-id"); // Uses specific method

        // All changes saved together
        await _unitOfWork.SaveChangesAsync();
    }
}

/* 
BENEFITS OF THIS GENERIC REPOSITORY PATTERN:

1. CODE REUSABILITY:
   - Common CRUD operations are implemented once
   - No need to repeat basic operations in each repository
   - Consistent API across all entities

2. MAINTAINABILITY:
   - Less code duplication
   - Centralized implementation of common operations
   - Easier to update common functionality

3. FLEXIBILITY:
   - Generic operations for simple cases
   - Specific operations for complex business logic
   - Easy to extend with new common operations

4. TYPE SAFETY:
   - Strongly typed operations
   - Compile-time checking
   - IntelliSense support

5. TESTABILITY:
   - Easy to mock repositories
   - Clear separation of concerns
   - Unit testing support

WHEN TO USE GENERIC VS SPECIFIC REPOSITORY:

GENERIC REPOSITORY - Use for:
✓ Basic CRUD operations (GetById, GetAll, Add, Update, Delete)
✓ Simple queries with predicates
✓ Counting and existence checks
✓ Simple includes

SPECIFIC REPOSITORY - Use for:
✓ Domain-specific business logic
✓ Complex queries with multiple joins
✓ Entity-specific validation
✓ Custom business operations
✓ Complex includes with business meaning

EXAMPLE USAGE PATTERN:
```csharp
// Generic for simple operations
var student = await _studentRepository.GetByIdAsync(id);
var allStudents = await _studentRepository.GetAllAsync();
await _studentRepository.AddAsync(newStudent);

// Specific for domain logic
var studentByEmail = await _studentRepository.GetByEmailAsync(email);
var activeStudents = await _studentRepository.GetActiveStudentsAsync();
```
*/
