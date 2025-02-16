using AutoMapper.Internal;
using LearnCrudAPI.Model;

namespace LearnCrudAPI.Service
{
    public interface IEmailService
    {
        Task SendEmail(Mailrequest mailrequest);
    }
}
