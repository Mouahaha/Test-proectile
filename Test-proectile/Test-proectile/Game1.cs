using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Test_proectile
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D pinguin, projectile, viseur;
        Rectangle positionPinguin, positionProjectile, rectanglePositionViseur;

        bool directionGauche = false;
        bool tir = false;

        //pour le viseur
        //centre du viseur
        Vector2 origineViseur;
        //position du viseur
        Vector2 positionViseur;
        //degres de rotation
        float rotationViseur;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //image du PERSO et son "rectangle position"
            pinguin = Content.Load<Texture2D>("pinguin");
            positionPinguin = new Rectangle(0, GraphicsDevice.Viewport.Height / 2 - pinguin.Height, pinguin.Width, pinguin.Height);

            //image du PROJECTILE et son "rectangle position"
            projectile = Content.Load<Texture2D>("projectile");
            positionProjectile = new Rectangle(positionPinguin.X + pinguin.Width, positionPinguin.Y, projectile.Width, projectile.Height);

            //image du VISEUR et son "rectangle position"
            viseur = Content.Load<Texture2D>("viseur");
            positionViseur = new Vector2(positionPinguin.X + pinguin.Width / 2 + viseur.Width / 2, positionPinguin.Y /2);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                //touche gauche -> direction a gauche vrai et deplacement a gauche
                positionPinguin.X -= 5;
                if (!tir)
                    directionGauche = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                //touche droite -> direction a gauche fausse et deplacement a droite
                positionPinguin.X += 5;
                if (!tir)
                    directionGauche = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //appui sur espace -> le tir s'active en fonction de la direction du perso(grace a directionGauche)
                if (!tir)
                {
                    //autre secteur a changer en cas de plusieur proectiles : appel de classe pour la trajectoire
                    if (directionGauche)
                    {
                        positionProjectile.X -= 10;
                    }
                    else
                        positionProjectile.X += 10;
                }
                tir = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Space) && tir == false)
            {
                //initialise la position du projectile avec celle du perso quand il na pas tire
                positionProjectile.X = positionPinguin.X + pinguin.Width;
                positionProjectile.Y = positionPinguin.Y;
            }

            //reinitialise le tir a faux si le proectile sort de la map ou etre en collision avec qqch(plus tard)
            if (positionProjectile.X > GraphicsDevice.Viewport.Width || positionProjectile.X < 0)
                tir = false;

            //continue la traectoire du projectile dans la bonne direction sans continuer l'appui
            if (tir && directionGauche)
                positionProjectile.X -= 10;     //secteur a changer en cas de differents projectiles(autre classe pour roquette, balle,...)
            if (tir && !directionGauche)
                positionProjectile.X += 10;

            //position du viseur de son carre a son vecteur
            positionViseur.X = positionPinguin.X + pinguin.Width / 2;   //defini la position du viseur par rappor au perso sans rotation
            positionViseur.Y = positionPinguin.Y + pinguin.Height / 2;  // se rafraichit souvent
            //defini taille et position du viseur
            rectanglePositionViseur = new Rectangle((int)positionViseur.X, (int)positionViseur.Y, (int)viseur.Width, (int)viseur.Height);
            //defini le centre de rotation et le rayon du cercle
            origineViseur = new Vector2(rectanglePositionViseur.Width + pinguin.Width / 2, rectanglePositionViseur.Height + pinguin.Height / 2);

            //bouttons de rotation du viseur autour du perso avec Haut Bas
            if (Keyboard.GetState().IsKeyUp(Keys.Up))
                rotationViseur += 0.1f;
            if (Keyboard.GetState().IsKeyUp(Keys.Down))
                rotationViseur -= 0.1f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            //dessin du perso
            spriteBatch.Draw(pinguin, positionPinguin, Color.White);
            //lancement du projectile
            if (tir)
                spriteBatch.Draw(projectile, positionProjectile, Color.White);
            //dessin du viseur
            spriteBatch.Draw(viseur, positionViseur, null, Color.White, rotationViseur, origineViseur, 1f, SpriteEffects.None, 0);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
