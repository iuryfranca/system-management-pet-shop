using SystemManagementPetshop.Context;
using SystemManagementPetshop.Models;
using Microsoft.EntityFrameworkCore;

namespace SystemManagementPetshop.Services;

public class AnimalService
{
    private readonly ContextDB _context;

    public AnimalService(ContextDB context)
    {
        _context = context;
    }

    public async Task<List<Animal>>? GetAnimais()
    {
        return await _context.Animais.ToListAsync();
    }

    public async Task<Animal>? GetAnimalById(int id)
    {
        return await _context.Animais.FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new Exception("Animal não encontrado");
    }

    public async Task<List<Animal>> GetAnimaisByTipo(TipoAnimal tipo)
    {
        return await _context.Animais.Where(p => p.TipoAni == tipo).ToListAsync();
    }

    public async Task<List<Animal>> GetAnimaisByDono(string donoNome)
    {
        return await _context.Animais.Where(p => p.DonoNome.Contains(donoNome)).ToListAsync();
    }

    public async Task<Animal>? CreateAnimal(Animal animal)
    {

        _context.ChangeTracker.Clear();

        await _context.Animais.AddAsync(animal);
        await _context.SaveChangesAsync();
        return animal;
    }

    public async Task DeleteAnimal(int id)
    {
        var animal = await GetAnimalById(id);
        _context.Animais.Remove(animal);
        await _context.SaveChangesAsync();
    }

    public async Task<Animal> UpdateAnimal(Animal animal)
    {
        if (animal == null)
        {
            throw new ArgumentNullException(nameof(animal));
        }

        bool animalExiste = await _context.Animais.AnyAsync(c => c.Id == animal.Id);
        if (!animalExiste)
        {
            throw new Exception($"Animal com ID {animal.Id} não encontrado");
        }

        _context.ChangeTracker.Clear();
        _context.Animais.Update(animal);
        await _context.SaveChangesAsync();

        return animal;
    }
}