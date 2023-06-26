using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICryptography
    {
        string BCryptEncrypt(string Password);
        bool BCryptVerify(string Password, string Hashed);
        string? Encrypt(string? Text);
        string? Decrypt(string? Text);
    }
}
