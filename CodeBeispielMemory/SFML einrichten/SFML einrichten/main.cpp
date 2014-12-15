#include <SFML/Graphics.hpp>
#include <ctime>
#include <iostream>

using namespace sf;

#define HEIGHT 4
#define WIDTH 6

struct Karte
{
	int id;
	bool show;
};

int main()
{
	RenderWindow window(VideoMode(640, 480), "Memorium");
	Sprite card;
	Texture cardtex;
	Karte spielfeld[HEIGHT][WIDTH];
	int frameWidth = 16, gezogeneKarten[12], cardsselected = 0;
	Vector2i selectedCard(99, 99);
	Vector2i selectedCard2(99, 99);
	bool canclick = true;

	srand(static_cast<int>(time(NULL)));
	cardtex.loadFromFile("set.png");
	card.setTexture(cardtex);

	//array initialisieren
	for (int i = 0; i < 12; i++)
	{
		gezogeneKarten[i] = 0;
	}

	//spielfeld fuellen
	for (int y = 0; y < HEIGHT; y++)
	{
		for (int x = 0; x < WIDTH; x++)
		{
			bool done = false;

			do
			{
				int tempnum = rand() % 12;

				if (gezogeneKarten[tempnum] < 2)
				{
					Karte neuekarte;
					neuekarte.id = tempnum;
					neuekarte.show = false;
					gezogeneKarten[tempnum] += 1;
					spielfeld[y][x] = neuekarte;
					done = true;
				}
			} while (!done);
		}
	}

	while(window.isOpen())
	{
		sf::Event event;

		while (window.pollEvent(event))
		{
			if (event.type == sf::Event::Closed)
				window.close();

			if (event.type == sf::Event::MouseButtonReleased && event.key.code == 0)
				canclick = true;
		}

		window.clear();

		for (int y = 0; y < HEIGHT; ++y)
		{
			for (int x = 0; x < WIDTH; ++x)
			{
				if (spielfeld[y][x].show)
				{
					card.setTextureRect(IntRect(frameWidth * spielfeld[y][x].id, 0, 16, 16));
				}
				else
				{
					card.setTextureRect(IntRect(frameWidth * 12, 0, 16, 16));
				}

				card.setScale(6.f, 6.f);
				//card.setScale(5.f, 5.f);
				card.setPosition(frameWidth * x * card.getScale().x, frameWidth * y * card.getScale().y);

				if (card.getGlobalBounds().intersects(Rect<float>(Mouse::getPosition(window).x,
					Mouse::getPosition(window).y,
					1.f,
					1.f)))
				{
					if (canclick && Mouse::isButtonPressed(Mouse::Left) && !spielfeld[y][x].show)
					{
						canclick = false;

						if (cardsselected == 2)
						{
							cardsselected = 0;
							spielfeld[selectedCard.y][selectedCard.x].show = false;
							spielfeld[selectedCard2.y][selectedCard2.x].show = false;
							std::cout << "Dont Match" << std::endl;
						}

						if (cardsselected == 0)
						{
							cardsselected++;

							spielfeld[y][x].show = true;
							selectedCard.x = x;
							selectedCard.y = y;
						}
						else if (cardsselected == 1)
						{
							cardsselected++;

							spielfeld[y][x].show = true;
							selectedCard2.x = x;
							selectedCard2.y = y;

							if (spielfeld[selectedCard.y][selectedCard.x].id == spielfeld[selectedCard2.y][selectedCard2.x].id)
							{
								cardsselected = 0;
								std::cout << "match!" << std::endl;
							}
						}
					}
				}

				window.draw(card);
			}
		}

		window.display();

	}
	
	return 0;
}