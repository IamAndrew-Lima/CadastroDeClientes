using CadastroClientes.Data;
using CadastroClientes.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroClientes.Repository
{
    public class ClientesRepository
    {
        private readonly BancoContext _context;

        public ClientesRepository(BancoContext context)
        {
            _context = context;
        }

        
        public async Task SalvarAsync(Clientes clienteNovo)
        {
            if (clienteNovo == null)
            {
                throw new ArgumentNullException(nameof(clienteNovo), "O cliente não pode ser nulo.");
            }

            
            var clienteExistente = await _context.ClientesCadastro
                                                 .FirstOrDefaultAsync(c => c.Documento == clienteNovo.Documento);

            if (clienteExistente != null)
            {
                
                clienteExistente.Nome = clienteNovo.Nome;
                clienteExistente.Sexo = clienteNovo.Sexo;
                clienteExistente.Email = clienteNovo.Email;
                clienteExistente.Telefone = clienteNovo.Telefone;
                clienteExistente.Fax = clienteNovo.Fax;
                clienteExistente.UF = clienteNovo.UF;
                
            }
            else
            {
                
                await _context.ClientesCadastro.AddAsync(clienteNovo);
            }

            
            await _context.SaveChangesAsync();
        }


        public async Task<bool> AlterarAsync(Clientes clientes)
        {
            if (clientes == null)
            {
                throw new ArgumentNullException(nameof(clientes), "O cliente não pode ser nulo.");
            }

            // Busca o cliente pelo documento
            var item = await _context.ClientesCadastro.FirstOrDefaultAsync(t => t.Documento == clientes.Documento);

            if (item != null)
            {
                item.IdCliente = clientes.IdCliente;
                item.Documento = clientes.Documento;
                item.Nome = clientes.Nome;
                item.Sexo = clientes.Sexo;
                item.Email = clientes.Email; 
                item.Telefone = clientes.Telefone;
                item.Fax = clientes.Fax;
                item.UF = clientes.UF;

                // Salva as alterações no contexto
                await _context.SaveChangesAsync();
                return true;
            }

            return false; // Cliente não encontrado
        }


        public async Task<List<Clientes>> ListarAsync()
        {
            // Retorna a lista de clientes ordenada pelo nome
            return await _context.ClientesCadastro.OrderByDescending(t => t.Nome).ToListAsync();
        }

        public async Task<bool> DeletarAsync(string documento)
        {
            if (string.IsNullOrEmpty(documento))
            {
                throw new ArgumentException("O documento não pode ser nulo ou vazio.", nameof(documento));
            }

            // Busca o cliente pelo documento
            var item = await _context.ClientesCadastro.FirstOrDefaultAsync(t => t.Documento == documento);

            if (item != null)
            {
                // Remove o cliente do contexto
                _context.ClientesCadastro.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Clientes> GetClienteAsync(string documento)
        {
            if (string.IsNullOrEmpty(documento))
            {
                throw new ArgumentException("O documento não pode ser nulo ou vazio.", nameof(documento));
            }

            // Busca e retorna o cliente pelo documento
            return await _context.ClientesCadastro.FirstOrDefaultAsync(t => t.Documento == documento);
        }
    }
}
