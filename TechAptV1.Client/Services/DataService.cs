// Copyright © 2025 Always Active Technologies PTY Ltd

using Microsoft.EntityFrameworkCore;
using TechAptV1.Client.DatabaseContext;
using TechAptV1.Client.Interface;
using TechAptV1.Client.Models;

namespace TechAptV1.Client.Services;

/// <summary>
/// Data Access Service for interfacing with the SQLite Database
/// </summary>
public sealed class DataService : IDataService
{
    private readonly DataContext _context;
    private readonly ILogger<DataService> _logger;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Default constructor providing DI Logger and Configuration
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    public DataService(ILogger<DataService> logger, IConfiguration configuration, DataContext context)
    {
        this._logger = logger;
        this._configuration = configuration;
        this._context = context;
    }

    /// <summary>
    /// Save the list of data to the SQLite Database
    /// </summary>
    /// <param name="dataList"></param>
    public async Task Save(List<Number> dataList)
    {

        _logger.LogInformation("Saving numbers to database.");

        try
        {
            _context.Numbers.AddRange(dataList);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Numbers saved successfully.");
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database error while saving numbers.");
            throw new Exception("Failed to save data due to a database error.", dbEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while saving numbers.");
            throw new Exception("Failed to save data due to an unexpected error.", ex);
        }
    }

    /// <summary>
    /// Fetch N records from the SQLite Database where N is specified by the count parameter
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public IEnumerable<Number> Get(int count)
    {
        _logger.LogInformation("Fetching numbers from database.");
        return _context.Numbers.Take(count).ToList();
    }

    /// <summary>
    /// Fetch All the records from the SQLite Database
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Number> GetAll()
    {
        _logger.LogInformation("Fetching all numbers from database.");
        return _context.Numbers.ToList();
    }
}
