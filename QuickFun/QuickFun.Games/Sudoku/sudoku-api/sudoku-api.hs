{-# LANGUAGE OverloadedStrings #-}
{-# LANGUAGE ScopedTypeVariables #-}

import Control.Applicative ((<|>))
import Control.Monad.IO.Class (liftIO)
import Data.Aeson (object, (.=))
import Data.List (delete, find)
import Network.Wai.Middleware.Cors
import System.Random (randomRIO)
import Web.Scotty hiding (delete)

type Board = [[Int]]

emptyBoard :: Board
emptyBoard = replicate 9 (replicate 9 0)

isValid :: Board -> Int -> Int -> Int -> Bool
isValid b r c v = validRow b r c v && validCol b r c v && validBox b r c v
  where
    validRow board row _ val = val `notElem` (board !! row)
    validCol board _ col val = val `notElem` [board !! i !! col | i <- [0 .. 8]]
    validBox board row col val = val `notElem` [board !! i !! j | i <- [boxR .. boxR + 2], j <- [boxC .. boxC + 2]]
      where
        (boxR, boxC) = ((row `div` 3) * 3, (col `div` 3) * 3)

setCell :: Board -> Int -> Int -> Int -> Board
setCell b r c v = take r b ++ [take c (b !! r) ++ [v] ++ drop (c + 1) (b !! r)] ++ drop (r + 1) b

findEmpty :: Board -> Maybe (Int, Int)
findEmpty b = find (\(r, c) -> b !! r !! c == 0) [(r, c) | r <- [0 .. 8], c <- [0 .. 8]]

countSolutions :: Board -> [Board]
countSolutions b = case findEmpty b of
  Nothing -> [b]
  Just (r, c) ->
    let validValues = [v | v <- [1 .. 9], isValid b r c v]
        solutions = concat [countSolutions (setCell b r c v) | v <- validValues]
     in take 2 solutions

shuffleList :: (Eq a) => [a] -> IO [a]
shuffleList [] = return []
shuffleList xs = do
  i <- randomRIO (0, length xs - 1)
  let x = xs !! i
  rest <- shuffleList (Data.List.delete x xs)
  return (x : rest)

fillBoard :: Board -> IO (Maybe Board)
fillBoard b = case findEmpty b of
  Nothing -> return (Just b)
  Just (r, c) -> do
    values <- shuffleList [1 .. 9]
    let try [] = return Nothing
        try (v : vs)
          | isValid b r c v = do
              res <- fillBoard (setCell b r c v)
              case res of
                Just sol -> return (Just sol)
                Nothing -> try vs
          | otherwise = try vs
    try values

reduceBoard :: Board -> [Int] -> Int -> IO Board
reduceBoard b [] _ = return b
reduceBoard b _ 0 = return b
reduceBoard b (idx : idxs) n = do
  let r = idx `div` 9
      c = idx `mod` 9
      val = b !! r !! c
      tempBoard = setCell b r c 0

  if length (countSolutions tempBoard) == 1
    then reduceBoard tempBoard idxs (n - 1)
    else reduceBoard b idxs n

data Difficulty = Easy | Medium | Hard | Test

difficultyToCells :: Difficulty -> Int
difficultyToCells Test = 1
difficultyToCells Easy = 30
difficultyToCells Medium = 45
difficultyToCells Hard = 55

parseDifficulty :: String -> Difficulty
parseDifficulty "test" = Test
parseDifficulty "easy" = Easy
parseDifficulty "medium" = Medium
parseDifficulty "hard" = Hard
parseDifficulty _ = Medium

generateSudoku :: Difficulty -> IO Board
generateSudoku diff = do
  Just full <- fillBoard emptyBoard
  indices <- shuffleList [0 .. 80]
  reduceBoard full indices (difficultyToCells diff)

main :: IO ()
main = scotty 3000 $ do
  middleware simpleCors
  get "/api/sudoku/generate" $ do
    diffParam <- param "difficulty" <|> return "medium"

    let diff = parseDifficulty diffParam
    board <- liftIO $ generateSudoku diff
    solved <- liftIO $ fillBoard board

    json $
      object
        [ "board" .= board,
          "difficulty" .= diffParam,
          "solution" .= solved
        ]

  get "/api/health" $ do
    json $ object ["status" .= ("ok" :: String)]