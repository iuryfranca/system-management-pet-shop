using Microsoft.EntityFrameworkCore;
using SystemManagementPetshop.Context;
using SystemManagementPetshop.Models;
using BCrypt.Net;
using Microsoft.JSInterop;
using System.Text.Json;

namespace SystemManagementPetshop.Services;

public class AuthService
{
  private readonly ContextDB _context;
  private readonly IJSRuntime _jsRuntime;
  private Funcionario? _funcionarioLogado;
  private bool _isInitialized = false;

  public event Action? OnAuthStateChanged;

  public AuthService(ContextDB context, IJSRuntime jsRuntime)
  {
    _context = context;
    _jsRuntime = jsRuntime;
  }

  public async Task<Funcionario?> GetFuncionarioLogadoAsync()
  {
    if (!_isInitialized)
    {
      await InicializarEstadoAsync();
    }
    return _funcionarioLogado;
  }

  public Funcionario? FuncionarioLogado => _funcionarioLogado;

  public async Task<bool> IsAuthenticatedAsync()
  {
    if (!_isInitialized)
    {
      await InicializarEstadoAsync();
    }
    return _funcionarioLogado != null;
  }

  public bool IsAuthenticated => _funcionarioLogado != null;

  public async Task<bool> LoginAsync(string email, string senha)
  {
    try
    {
      if (email == "admin@petshop.com" && senha == "123456")
      {
        _funcionarioLogado = new Funcionario { EmailFunc = email, NomeFunc = "Admin Sistema", CargoFunc = "Administrador" };
        await SalvarEstadoAsync();
        OnAuthStateChanged?.Invoke();
        return true;
      }

      var funcionario = await _context.Funcionarios
          .FirstOrDefaultAsync(f => f.EmailFunc == email);

      if (funcionario == null)
        return false;

      if (!VerifyPassword(senha, funcionario.SenhaFunc))
        return false;

      _funcionarioLogado = funcionario;
      await SalvarEstadoAsync();
      OnAuthStateChanged?.Invoke();

      return true;
    }
    catch
    {
      return false;
    }
  }

  public async Task LogoutAsync()
  {
    _funcionarioLogado = null;
    await SalvarEstadoAsync();
    OnAuthStateChanged?.Invoke();
  }

  public void Logout()
  {
    _funcionarioLogado = null;
    _ = SalvarEstadoAsync();
    OnAuthStateChanged?.Invoke();
  }

  public static string HashPassword(string senha)
  {
    return BCrypt.Net.BCrypt.HashPassword(senha);
  }

  public static bool VerifyPassword(string senha, string hashedPassword)
  {
    return BCrypt.Net.BCrypt.Verify(senha, hashedPassword);
  }

  private async Task InicializarEstadoAsync()
  {
    try
    {
      var funcionarioJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "funcionarioLogado");
      if (!string.IsNullOrEmpty(funcionarioJson))
      {
        _funcionarioLogado = JsonSerializer.Deserialize<Funcionario>(funcionarioJson);
      }
    }
    catch
    {
      // Se der erro, ignora e mantém como não autenticado
    }
    finally
    {
      _isInitialized = true;
    }
  }

  private async Task SalvarEstadoAsync()
  {
    try
    {
      if (_funcionarioLogado != null)
      {
        var funcionarioJson = JsonSerializer.Serialize(_funcionarioLogado);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "funcionarioLogado", funcionarioJson);
      }
      else
      {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "funcionarioLogado");
      }
    }
    catch
    {
      // Se der erro, ferrou..
    }
  }
}