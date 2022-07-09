using Avaliacao3BimLp3.Database;
using Avaliacao3BimLp3.Models;
using Avaliacao3BimLp3.Repositories;

var databaseConfig = new DatabaseConfig();

var databaseSetup = new DatabaseSetup(databaseConfig);

var studentRepository = new StudentRepository(databaseConfig);

var modelName = args[0];
var modelAction = args[1];

if (modelName == "Student")
{
    //3.1
    if (modelAction == "New")
    {
        string registration = args[2];
        string name = args[3];
        string city = args[4];
        
        if(studentRepository.ExistsById(registration))
        {
            Console.WriteLine($"Estudante com Id {registration} já existe");
        } else 
{
            var student = new Student(registration, name, city, false);
            studentRepository.Save(student);
            Console.WriteLine($"Estudante {name} cadastrado com sucesso");
        }
    }
    //3.2
    if(modelAction == "Delete")
    {
        var registration = args[2];
        if(studentRepository.ExistsById(registration))
        {
            studentRepository.Delete(registration);
            Console.WriteLine($"Estudante {registration} removido com sucesso");
        } else 
        {
            Console.WriteLine($"Estudante {registration} não encontrado");
        }
    }

    //3.3
    if(modelAction == "MarkAsFormed")
    {
        string registration = args[2];
        if(studentRepository.ExistsById(registration))
        {
            studentRepository.MarkAsFormed(registration);
            Console.WriteLine($"Estudante {registration} definido como formado");
        
        } else 
        {
            Console.WriteLine($"Estudante {registration} não encontrado");
        }
    }

    //3.4
    if(modelAction == "List")
    {
        if(studentRepository.GetAll().Count() > 0)
        {
            foreach (var student in studentRepository.GetAll())
            {
                if(student.Former)
                {
                    Console.WriteLine($"{student.Registration}, {student.Name}, {student.City}, Formado");
                } else {
                    Console.WriteLine($"{student.Registration}, {student.Name}, {student.City}, Não formado");
                }
            }
        } else {
            throw new ArgumentException($"Nenhum estudante cadastrado.");
        }
    }

    //3.5
    if(modelAction == "ListFormed")
    {
        if(studentRepository.GetAllFormed().Count == 0)
        {
            Console.WriteLine("Nenhum estudante formado cadastrado");
        } else {
            foreach (var student in studentRepository.GetAllFormed())
            {
                Console.WriteLine($"{student.Registration}, {student.Name}, {student.City}, formado");
            }
        }
    }

    //3.6
    if(modelAction == "ListByCity")
    {
        if(studentRepository.GetAllStudentByCity(args[2]).Count == 0)
        {
            Console.WriteLine("Nenhum estudante cadastrado nessa cidade");
        } else {
            foreach (var student in studentRepository.GetAllStudentByCity(args[2]))
            {
                if(student.Former)
                {
                    Console.WriteLine($"{student.Registration}, {student.Name}, {student.City}, formado");
                } else {
                    Console.WriteLine($"{student.Registration}, {student.Name}, {student.City}, não formado");
                }  
            }
        }
    }

    //3.7
    if(modelAction == "ListByCities")
    {
        string[] vetor = new string[args.Length - 2];
        for(int i = 2; i < args.Length; i++)
        {
            vetor[i-2] = args[i];
        } 
        if (studentRepository.GetAllByCities(vetor).Count == 0)
        {
            Console.WriteLine("Nenhum estudante cadastrado nessas cidades");
        } else {
            foreach (var student in studentRepository.GetAllByCities(vetor))
            {
                if(student.Former)
                {
                    Console.WriteLine($"{student.Registration}, {student.Name}, {student.City}, formado");
                } else {
                    Console.WriteLine($"{student.Registration}, {student.Name}, {student.City}, não formado");
                }      
            }
        }
    }

    //3.8 - 3.9
    if(modelAction == "Report")
    {
        if(args[2] == "CountByCities")
        {
            Console.WriteLine("Student CountByCities");
            if(studentRepository.CountByCities().Count == 0)
            {
                Console.WriteLine($"Nenhum estudante cadastrado");
            } else {
                foreach (var student in studentRepository.CountByCities())
                {
                    Console.WriteLine($"{student.AttributeName}, {student.StudentNumber}");       
                }
            }
        }

        if(args[2] == "CountByFormed")
        {
            Console.WriteLine("Student CountByFormed");
            if(studentRepository.CountByFormed().Count == 0)
            {
                Console.WriteLine($"Nenhum estudante cadastrado");
            } else {
                foreach (var student in studentRepository.CountByFormed())
                {
                    if(student.AttributeName == "1")
                    {
                        Console.WriteLine($"Formado, {student.StudentNumber}");
                    } else {
                        Console.WriteLine($"Não Formado, {student.StudentNumber}");
                    }    
                }
            }
        }
    }
}