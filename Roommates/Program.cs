using System;
using System.Collections.Generic;
using System.Linq;

using Roommates.Models;
using Roommates.Repositories;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for a room"):
                        Console.Write("Room Id: ");
                        int roomId = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(roomId);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string roomName = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = roomName,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Update a room"):
                        List<Room> roomOptions = roomRepo.GetAll();
                        foreach (Room r in roomOptions)
                        {
                            Console.WriteLine($"{r.Id} - {r.Name} Max Occupancy({r.MaxOccupancy})");
                        }

                        Console.Write("Which room would you like to update? ");
                        int selectedRoomId = int.Parse(Console.ReadLine());
                        Room selectedRoom = roomOptions.FirstOrDefault(r => r.Id == selectedRoomId);

                        Console.Write("New Name: ");
                        selectedRoom.Name = Console.ReadLine();

                        Console.Write("New Max Occupancy: ");
                        selectedRoom.MaxOccupancy = int.Parse(Console.ReadLine());

                        roomRepo.Update(selectedRoom);

                        Console.WriteLine("Room has been successfully updated");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Name} has an Id of {c.Id}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("View unassigned chores"):
                        List<Chore> unassignedChores = choreRepo.GetUnnasignedChores();
                        Console.WriteLine("The following chores are unassigned:");
                        int idx = 1;
                        foreach (Chore uc in unassignedChores)
                        {
                            Console.WriteLine($"{idx}. {uc.Name}");
                            idx++;
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Assign chore to roommate"):
                        // list unassigned chores
                        List<Chore> unassignedChoreList = choreRepo.GetUnnasignedChores();
                        Console.WriteLine("The following chores are unassigned:");
                        idx = 1;
                        foreach (Chore uc in unassignedChoreList)
                        {
                            Console.WriteLine($"{idx}. {uc.Name}");
                            idx++;
                        }

                        // allow the user to select a chore
                        Chore selectedChore;
                        while (true)
                        {
                            try
                            {
                                Console.WriteLine();
                                Console.Write("Select a chore > ");

                                string input = Console.ReadLine();
                                int index = int.Parse(input) - 1;
                                selectedChore = unassignedChoreList[index];
                                break;
                            }
                            catch (Exception)
                            {

                                continue;
                            }
                        }

                        
                        // list all roommates
                        Console.WriteLine();
                        Console.WriteLine("Roommates: ");
                        Console.WriteLine();
                        
                        List<Roommate> roommates = roommateRepo.GetAll();
                        idx = 1;
                        foreach (Roommate r in roommates)
                        {
                            Console.WriteLine($"{idx}. {r.FirstName}");
                            idx++;
                        }

                        // allow the user to select a roommate
                        Roommate selectedRoommate;
                        while (true)
                        {
                            try
                            {
                                Console.WriteLine();
                                Console.Write($"Select a roommate to assign to '{selectedChore.Name}' > ");

                                string input = Console.ReadLine();
                                int index = int.Parse(input) - 1;
                                selectedRoommate = roommates[index];
                                break;
                            }
                            catch (Exception)
                            {

                                continue;
                            }
                        }

                        // assign the chore in the db
                        choreRepo.AssignChore(selectedRoommate.Id, selectedChore.Id);

                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for a chore"):
                        Console.Write("Chore Id: ");
                        int choreId = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(choreId);

                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName,
                        };

                        choreRepo.Insert(choreToAdd);
                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for a roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());
                        Roommate roommate = roommateRepo.GetById(roommateId);
                        Console.WriteLine($"{roommate.Id} - {roommate.FirstName} - {roommate.RentPortion}% - {roommate.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }

        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for a room",
                "Add a room",
                "Update a room",
                "Show all chores",
                "View unassigned chores",
                "Assign chore to roommate",
                "Search for a chore",
                "Add a chore",
                "Search for a roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}