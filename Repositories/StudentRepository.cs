using Avaliacao3BimLp3.Models;
using Microsoft.Data.Sqlite;
using Avaliacao3BimLp3.Database;
using Dapper;

namespace Avaliacao3BimLp3.Repositories;

class StudentRepository
{
    private readonly DatabaseConfig _databaseConfig;
    public StudentRepository(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    //Insere um estudante na tabela
    public Student Save(Student student)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("INSERT INTO Students VALUES(@Registration, @Name, @City, @Former)",student);

        return student;
    }

    //Verificar se ID já esta cadastrado
    public bool ExistsById(string registration)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var result = connection.ExecuteScalar<Boolean>("SELECT count(registration) FROM students WHERE (registration = @Registration)", new{ Registration = registration});

        return result;
    }

    //Deleta um estudante 
    public void Delete(string registration)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("DELETE FROM Students WHERE (registration = @Registration)", new { Registration = registration});
    }

    //Marca um estudante como formado
    public void MarkAsFormed(string registration)
    {
        using var connection = new SqliteConnection( _databaseConfig.ConnectionString);
        connection.Open();

        connection.Execute("UPDATE Students SET former == true WHERE (registration = @Registration)", new { Registration = registration });

    }

    //Retorna todos os estudantes
    public List<Student> GetAll()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var students = connection.Query<Student>("SELECT * FROM Students").ToList();

        return students;
    }

    //Retorna todos os estudantes formados
    public List<Student> GetAllFormed()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();
        
        var students = connection.Query<Student>("SELECT * FROM Students WHERE former = true");

        return students.ToList();
    }

    //Retorna todos estudantes de uma cidade
    public List<Student> GetAllStudentByCity(string city)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var students = connection.Query<Student>("SELECT * FROM Students WHERE city LIKE @City", new { City = $"{city}%"});

        return students.ToList();
    }


    //Retorna os estudantes das cidades presentes no array cities
    public List<Student> GetAllByCities(string[] cities)
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var students = connection.Query<Student>("SELECT * FROM Students WHERE city IN @City", new { City = cities});

        return students.ToList();
    }

    //Retornr o número de estudantes agrupados por cidade
    public List<CountStudentGroupByAttribute> CountByCities()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var students = connection.Query<CountStudentGroupByAttribute>("SELECT city as AttributeName, COUNT(city) as StudentNumber FROM Students GROUP BY city");
    
        return students.ToList();
    }

    //Retorna o número de estudantes agrupados por formados e não formados
    public List<CountStudentGroupByAttribute> CountByFormed()
    {
        using var connection = new SqliteConnection(_databaseConfig.ConnectionString);
        connection.Open();

        var students = connection.Query<CountStudentGroupByAttribute>("SELECT * FROM Students WHERE former = true");

        return students.ToList();
    }
}
