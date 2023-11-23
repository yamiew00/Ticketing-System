db.getCollection("TransactionRecord").aggregate([
{
    $unwind: '$Tickets'
},
{
    $group:
    {
        _id: '$Tickets',
        count: {$sum: 1}
    }
},
{
    $match: {count: {$eq: 1}}
},
{
    $group:
    {
        _id: null,
        total: {$sum: 1}
    }
},
{
    $project: 
    {
        _id: 0,
        total: 1    
    }
}
])
