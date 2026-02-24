namespace QuickFun.Domain.Entities;
//Jest to DTO czyli Data Transfer Object
//obiekt dzieki ktoremu bedziemy przekazywac tylko potrzebne informacje o wyniku i grze
public record GameResultRequest (int Score, QuickFun.Domain.Enums.GameType GameType);
